'use client'


import { useState } from "react"
import Image from "next/image"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import Modal, {DialogDemo} from "@/components/modalComponent";

export function CourseCard({ course }) {
    const [showEvaluationForm, setShowEvaluationForm] = useState(false)
    const [rating, setRating] = useState(0)
    const [comment, setComment] = useState("")
    const [hoverRating, setHoverRating] = useState(0)

    const handleSubmit = (e) => {
        e.preventDefault()
        // Soumettre l'évaluation
        console.log({ rating, comment })
        setShowEvaluationForm(false)
    }

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

                {/* Section d'évaluation */}
                {showEvaluationForm && (
                    <form onSubmit={handleSubmit} className="mt-4 space-y-4 border-t pt-4">
                        <div>
                            <label className="block text-sm font-medium mb-2">Notez ce cours</label>
                            <div className="flex gap-1">
                                {[1, 2, 3, 4, 5].map((star) => (
                                    <button
                                        key={star}
                                        type="button"
                                        className={`text-2xl ${star <= rating ? 'text-yellow-400' : 'text-gray-300'} hover:text-yellow-500`}
                                        onClick={() => setRating(star)}
                                    >
                                        ★
                                    </button>
                                ))}
                            </div>
                        </div>

                        <div>
                            <label className="block text-sm font-medium mb-2">Commentaire</label>
                            <textarea
                                className="w-full p-2 border rounded-md focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                rows="3"
                                value={comment}
                                onChange={(e) => setComment(e.target.value)}
                                placeholder="Partagez votre expérience..."
                            />
                        </div>

                        <div className="flex gap-2 justify-end">
                            <button
                                type="button"
                                onClick={() => setShowEvaluationForm(false)}
                                className="px-4 py-2 text-sm text-gray-600 hover:text-gray-800"
                            >
                                Annuler
                            </button>
                            <button
                                type="submit"
                                className="px-4 py-2 text-sm bg-purple-600 text-white rounded-md hover:bg-purple-700"
                            >
                                Envoyer
                            </button>
                        </div>
                    </form>
                )}

                <div className="mb-4 flex items-center justify-between text-sm text-gray-500">
                    <div className="flex items-center gap-1">
                        <ClockIcon />
                        <span>{course.duration}</span>
                    </div>
                    <div className="flex items-center gap-1">
                        <UserIcon />
                        <span>{course.students}</span>
                    </div>
                </div>
                <div className="relative">
                    <div className="flex items-center justify-between">
                        <div className="flex items-center gap-2">
                            <Avatar className="h-8 w-8">
                                <AvatarImage src="/placeholder-user.jpg" />
                                <AvatarFallback>PF</AvatarFallback>
                            </Avatar>
                            <span className="text-sm text-gray-600">{course.professor}</span>
                        </div>

                        <DialogDemo
                            title="Evaluate"
                            course={course}
                        />
                    </div>
                </div>
                {/*test using modal*/}






            </div>
        </div>
    )
}

// Les composants d'icônes restent identiques...
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

function StarIconHalf() {
    return (
        <svg
            xmlns="http://www.w3.org/2000/svg"
            width="16"
            height="16"
            viewBox="0 0 24 24"
            stroke="currentColor"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
        >
            <path d="M12 17.8 5.8 21 7 14.1 2 9.3l7-1L12 2" fill="currentColor" />
            <path d="M12 2v15.8l3.2-1.7" fill="none" />
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

function UserIcon() {
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
            <path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2" />
            <circle cx="12" cy="7" r="4" />
        </svg>
    )
}









