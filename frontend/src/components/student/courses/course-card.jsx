'use client'

import { useState, useEffect } from "react"
import Image from "next/image"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { DialogDemo } from "@/components/student/courses/modalComponent";
import { getUserIdFromCookie } from "@/lib/utils";

export function CourseCard({ course, evaluation }) {
  console.log(evaluation);
    let  score=0
    let  commentaire=""
    let idEvaluation=0
    if(evaluation.length>0) {
       let ev = evaluation[0]
        score = ev.score
        commentaire = ev.comment
        idEvaluation = ev.evaluationId
    }

    const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;

    // Assurez-vous que le score est un entier valide entre 0 et 5
    const validScore = Math.max(0, Math.min(5,  Math.floor(score)));

    // console.log("Valid Score:", evaluation.score);

    // Générer les étoiles
    const renderStars = (count) => {
        return [...Array(count)].map((_, i) => (
            <button
                key={`star-${i}`}
                className="hover:scale-110 transition-transform"
                type="button"
            >
                {/* Affichage des étoiles en fonction du score */}
                <StarIcon className={`w-5 h-5 ${i < validScore ? 'text-yellow-400' : 'text-gray-400'}`} />
            </button>
        ));
    };

    return (
        <div className="overflow-hidden rounded-lg border border-gray-200 shadow-sm">
            <div className="relative h-70 w-full overflow-hidden">
                <Image
                    src={course.profileImageUrl ? `${baseUrl}${course.profileImageUrl}` : "/placeholder.svg"}
                    alt={course.moduleName}
                    fill
                    className="object-cover"
                />
            </div>
            <div className="p-4">
                <h3 className="mb-2 font-medium">{course.moduleName}</h3>
                <div className="mb-3 flex text-yellow-400">
                    {renderStars(5)} {/* Affichage des étoiles */}
                </div>
                <div className="mb-4 flex items-center justify-between text-sm text-gray-500">
                    <div className="flex items-center gap-1">
                        <ClockIcon />
                        <span>{course.moduleDuration}</span>
                    </div>
                </div>
                <div className="relative">
                    <div className="flex items-center justify-between">
                        <div className="flex items-center gap-2">
                            <Avatar className="h-8 w-8">
                                <AvatarImage src="/placeholder-user.jpg" />
                                <AvatarFallback>?</AvatarFallback>
                            </Avatar>
                            <span className="text-sm text-gray-600">{course.teacherFullName}</span>
                        </div>

                        <DialogDemo
                            title={validScore ? "Reevalue" : "Evaluate"}
                            course={course}
                            score={validScore}
                            commentaire={commentaire}
                            idEval={idEvaluation}
                        />
                    </div>
                </div>
            </div>
        </div>
    );
}

// Helper components for icons
function StarIcon({ className }) {
    return (
        <svg
            xmlns="http://www.w3.org/2000/svg"
            width="16"
            height="16"
            viewBox="0 0 24 24"
            className={className} // Pass the className here to control the color
            fill="none"
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
