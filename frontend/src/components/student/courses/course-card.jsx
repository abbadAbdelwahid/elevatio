'use client'

import Image from "next/image"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { DialogDemo } from "@/components/student/courses/modalComponent";
import { memo } from "react";

export const CourseCard = memo(({ course, evaluations, onRefresh }) => {
    const { score = 0, comment = "", evaluationId = 0 } = evaluations[0] || {};
    const validScore = Math.max(0, Math.min(5, Math.floor(score)));
    const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;

    const renderStars = () => {
        return Array(5).fill(0).map((_, i) => (
            <span key={`star-${i}`} className={`text-2xl ${i < validScore ? 'text-yellow-400' : 'text-gray-300'}`}>
                ★
            </span>
        ));
    };

    return (
        <div className="overflow-hidden rounded-lg border border-gray-200 shadow-sm transition-all hover:shadow-md">
            <div className="relative h-48 w-full overflow-hidden">
                <Image
                    src={course.profileImageUrl ? `${baseUrl}${course.profileImageUrl}` : "/placeholder.svg"}
                    alt={course.moduleName}
                    fill
                    className="object-cover"
                    sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
                />
            </div>
            <div className="p-4">
                <h3 className="mb-2 font-medium line-clamp-2">{course.moduleName}</h3>
                <div className="mb-3 flex">
                    {renderStars()}
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
                            <span className="text-sm text-gray-600 line-clamp-1">{course.teacherFullName}</span>
                        </div>
                        <DialogDemo
                            title={validScore ? "Réévaluer" : "Évaluer"}
                            course={course}
                            score={validScore}
                            commentaire={comment}
                            idEval={evaluationId}
                            onSuccess={onRefresh}
                        />
                    </div>
                </div>
            </div>
        </div>
    );
});

const ClockIcon = () => (
    <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
        <circle cx="12" cy="12" r="10" />
        <polyline points="12 6 12 12 16 14" />
    </svg>
);

CourseCard.displayName = 'CourseCard';