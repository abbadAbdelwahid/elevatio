'use client'


import { useState} from "react"
import Image from "next/image"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import  {DialogDemo} from "@/components/student/courses/modalComponent";

export function CourseCard({ course }) {

    const [rating, setRating] = useState(course.evaluation?.rating || 0)
    const [hoverRating, setHoverRating] = useState(0)

    // Générer les étoiles
    const renderStars = (count, filled) => {
        return [...Array(count)].map((_, i) => (
            <button
                key={`${filled ? 'filled' : 'empty'}-${i}`}
                className="hover:scale-110 transition-transform"
                onClick={() => setRating(filled ? i + 1 : rating)}
                onMouseEnter={() => setHoverRating(filled ? i + 1 : 0)}
                onMouseLeave={() => setHoverRating(0)}
            >
                <StarIcon filled={filled && (i < rating || i < hoverRating)} />
            </button>
        ))
    }

    return (
        <div className="overflow-hidden rounded-lg border border-gray-200 shadow-sm">
            <div className="relative h-70 w-full overflow-hidden">
                <Image src={course.image || "/placeholder.svg"} alt={course.title} fill className="object-cover" />
            </div>
            <div className="p-4">
                <h3 className="mb-2 font-medium">{course.title}</h3>
                <div className="mb-3 flex text-yellow-400">
                    {renderStars(5, true)}
                </div>
                <div className="mb-4 flex items-center justify-between text-sm text-gray-500">
                    <div className="flex items-center gap-1">
                        <ClockIcon />
                        <span>{course.duration}</span>
                    </div>
                </div>
                <div className="relative">
                    <div className="flex items-center justify-between">
                        <div className="flex items-center gap-2">
                            <Avatar className="h-8 w-8">
                                <AvatarImage src="/placeholder-user.jpg" />
                                <AvatarFallback>{}</AvatarFallback>
                            </Avatar>
                            <span className="text-sm text-gray-600">{course.professor}</span>
                        </div>

                        <DialogDemo
                            title={course.evaluation ? "Reevalue" : "Evaluate"}
                            course={course}
                        />
                    </div>
                </div>
                {/*test using modal*/}






            </div>
        </div>
    )
}


// Helper components for icons
function StarIcon({ filled }) {
    return (
        <svg
            xmlns="http://www.w3.org/2000/svg"
            width="16"
            height="16"
            viewBox="0 0 24 24"
            fill={filled ? "currentColor" : "none"}
            stroke="currentColor"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
        >
            <polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2" />
        </svg>
    )
}

function ClockIcon() {
    return (
        <svg
            xmlns="http://www.w3.org/2000/svg"
            width="14"
            height="14"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
        >
            <circle cx="12" cy="12" r="10" />
            <polyline points="12 6 12 12 16 14" />
        </svg>
    )
}











