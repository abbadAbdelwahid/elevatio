'use client'

import { useState, useEffect, useCallback } from 'react';
import { CourseGrid } from "@/components/student/courses/course-grid";
import { SearchHeader } from "@/components/layout/search-header";
import Loading from "@/components/loading/loading";
import { CourseFilters } from "@/components/student/courses/course-filters";
import { getUserIdFromCookie } from "@/lib/utils";

export default function CoursesPage() {
    const [courses, setCourses] = useState([]);
    const [activeFilter, setActiveFilter] = useState("All");
    const [isLoading, setIsLoading] = useState(true);
    const [ratings, setRatings] = useState({});

    const fetchData = useCallback(async () => {
        setIsLoading(true);
        try {
            const [coursesRes, ratingsRes] = await Promise.all([
                fetch(`${process.env.NEXT_PUBLIC_API_COURSE_URL}/api/Module?filter=${activeFilter}`),
                fetch(`${process.env.NEXT_PUBLIC_API_EVALUATION_URL}/api/evaluations/getEvaluationsByRespondentId/${getUserIdFromCookie()}`, {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
                        "Content-Type": "application/json"
                    }
                })
            ]);

            if (!coursesRes.ok || !ratingsRes.ok) {
                throw new Error('Failed to fetch data');
            }

            const [coursesData, evaluations] = await Promise.all([
                coursesRes.json(),
                ratingsRes.json()
            ]);

            const ratingsData = {};
            evaluations.forEach(evaluation => {
                if (evaluation.type === "Module") {
                    if (!ratingsData[evaluation.moduleId]) {
                        ratingsData[evaluation.moduleId] = [];
                    }
                    ratingsData[evaluation.moduleId].push(evaluation);
                }
            });

            setCourses(coursesData);
            setRatings(ratingsData);
        } catch (error) {
            console.error("Error fetching data:", error);
        } finally {
            setIsLoading(false);
        }
    }, [activeFilter]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                <CourseFilters
                    activeFilter={activeFilter}
                    setActiveFilter={setActiveFilter}
                />
                <div className="relative mt-8 min-h-[500px]">
                    {isLoading && (
                        <div className="absolute inset-0 z-10">
                            <Loading />
                        </div>
                    )}
                    <CourseGrid
                        courses={courses}
                        ratings={ratings}
                        onRefresh={fetchData}
                    />
                </div>
            </div>
        </div>
    );
}