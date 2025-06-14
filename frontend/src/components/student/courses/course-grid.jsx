'use client'

import { Suspense, memo } from "react";
import { CourseCard } from "./course-card";

export const CourseGrid = memo(({ courses, ratings, onRefresh }) => {
    return (
        <div className="p-14 max-h-[80vh] overflow-y-auto">
            <Suspense fallback={<div className="text-center py-4">Chargement...</div>}>
                <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-4">
                    {courses.map((course) => (
                        <CourseCard
                            key={course.moduleId}
                            course={course}
                            evaluations={ratings[course.moduleId] || []}
                            onRefresh={onRefresh}
                        />
                    ))}
                </div>
            </Suspense>
        </div>
    );
});

CourseGrid.displayName = 'CourseGrid';