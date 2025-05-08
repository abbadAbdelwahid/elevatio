import { CourseCard } from "./course-card"

// Mock data for courses
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

export function CourseGrid() {
    return (
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
