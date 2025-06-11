'use client'

import { useState, useEffect } from 'react';
import { CourseGrid } from "@/components/student/courses/course-grid"
import { SearchHeader } from "@/components/layout/search-header"
import Loading from "@/components/loading/loading";
import { CourseFilters } from "@/components/student/courses/course-filters";
import {getUserIdFromCookie} from "@/lib/utils";

export default function CoursesPage() {
    const [courses, setCourses] = useState([]);
    const [filteredCourses, setFilteredCourses] = useState([]);
    const [activeFilter, setActiveFilter] = useState("All");
    const [isLoading, setIsLoading] = useState(true);
    const [ratings, setRatings] = useState({});

    // Fonction pour récupérer les cours filtrés
    const fetchCourses = async (filter) => {
        setIsLoading(true);
        try {
            const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;
            const res = await fetch(`${baseUrl}/api/Module?filter=${filter}`);
            const data = await res.json();
            // console.log('les modules pour student: ',data);
            setCourses(data);
            setFilteredCourses(data); // Initialisation des cours filtrés par défaut (All)
        } catch (error) {
            console.error("Erreur de récupération des données", error);
        } finally {
            setIsLoading(false);
        }
    };

    // Charger les cours initialement et chaque fois que le filtre change
    useEffect(() => {
        fetchCourses(activeFilter);
    }, [activeFilter]);
    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                <CourseFilters activeFilter={activeFilter} setActiveFilter={setActiveFilter} />

                <div className="relative mt-8 min-h-[500px]">
                    {isLoading && (
                        <div className="absolute inset-0 z-10">
                            <Loading />
                        </div>
                    )}

                    {/* Passer les cours filtrés à CourseGrid */}
                    <CourseGrid courses={filteredCourses}  />
                </div>
            </div>
        </div>
    )
}
