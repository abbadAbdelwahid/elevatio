'use client'
import { CourseCard } from "./course-card"

import { useEffect, useState, } from "react"
// Mock data for courses

export function CourseGrid({courses}) {



        return(


            <div className="p-14 max-h-[80vh] overflow-y-auto">
                {/* Grid Layout avec gestion de la taille dynamique */}
                <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-4">
                    {courses.map((course) => (
                        <CourseCard key={course.id} course={course} />
                    ))}
                </div>
            </div>
        )



}
