'use client'
import {Suspense, useEffect, useState} from "react";
import { CourseCard } from "./course-card";

export function CourseGrid({ courses }) {
  console.log(courses)
    return (
        <div className="p-14 max-h-[80vh] overflow-y-auto">

            {/* Grid Layout avec gestion de la taille dynamique */}
            <Suspense fallback={<p>Loading feed...</p>}>
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-4">
                {courses.map((course) => (

                           <CourseCard key={course.id} course={course} />

                ))}
            </div>
            </Suspense >
        </div>
    );
}
