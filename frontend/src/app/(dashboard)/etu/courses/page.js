// 'use client'

// import { useState, useEffect } from 'react';
// import { CourseFilters } from "@/components/student/courses/course-filters"
import { CourseGrid } from "@/components/student/courses/course-grid"
import { SearchHeader } from "@/components/layout/search-header"
const courses = [
    {
        id: 1,
        title: "Programming Language C++",
        image: "/images/acceuil/about.svg",
        rating: 4.5,
        duration: "19h 30m",
        students: "50+ Student",
        professor: "Pr.fissoune",
    },
    {
        id: 2,
        title: "Programming Language C++",
        image: "/images/acceuil/about.svg",
        rating: 4.2,
        duration: "19h 30m",
        students: "50+ Student",
        professor: "Pr.fissoune",
    },
    {
        id: 3,
        title: "Programming Language C++",
        image: "/images/acceuil/about.svg",
        rating: 4.3,
        duration: "19h 30m",
        students: "50+ Student",
        professor: "Pr.fissoune",
    },
    {
        id: 4,
        title: "Programming Language C++",
        image: "/images/acceuil/about.svg",
        rating: 4.3,
        duration: "19h 30m",
        students: "50+ Student",
        professor: "Pr.fissoune",
    },
    {
        id: 5,
        title: "Programming Language C++",
        image: "/images/acceuil/about.svg",
        rating: 4.2,
        duration: "19h 30m",
        students: "50+ Student",
        professor: "Pr.fissoune",
    },
    {
        id: 6,
        title: "Programming Language C++",
        image: "/images/acceuil/about.svg",
        rating: 4.3,
        duration: "19h 30m",
        students: "50+ Student",
        professor: "Pr.fissoune",
    },
    {
        id: 7,
        title: "Programming Language C++",
        image: "/images/acceuil/about.svg",
        rating: 4.3,
        duration: "19h 30m",
        students: "50+ Student",
        professor: "Pr.fissoune",
    },
    {
        id: 8,
        title: "Programming Language C++",
        image: "/images/acceuil/about.svg",
        rating: 4.5,
        duration: "19h 30m",
        students: "50+ Student",
        professor: "Pr.fissoune",
    },
]


export default function CoursesPage() {


    // const [courses, setCourses] = useState([]);
    // useEffect(() => {
    //     const fetchCourses = async () => {
    //         try {
    //             // Si tu utilises un fichier db.json, assure-toi que tu l'ouvres via un serveur ou un proxy local
    //             const res = await fetch('http://localhost:3003/courses'); // URL de l'API ou fichier JSON
    //             const data = await res.json();
    //             setCourses(data); // Stocke les données dans l'état
    //         } catch (error) {
    //             console.error("Erreur de récupération des données", error);
    //         }
    //     };
    //
    //     fetchCourses(); // Appel de la fonction
    // }, []);

    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            {/* Main content */}
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                {/*<CourseFilters  />*/}
                <CourseGrid coursess={courses}  />

            </div>
        </div>
    )
}
