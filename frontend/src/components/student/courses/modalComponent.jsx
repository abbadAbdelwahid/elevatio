'use client'

import { useState, useRef, useEffect } from "react"
import { Button } from "@/components/ui/button"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import { Textarea } from "@/components/ui/textarea"
import { getUserIdFromCookie } from "@/lib/utils"
import { toast } from "sonner"

export function DialogDemo({ title, course, score = 0, commentaire = "", idEval = 0, onSuccess }) {
    const [rating, setRating] = useState(score)
    const [comment, setComment] = useState(commentaire)
    const [isSubmitting, setIsSubmitting] = useState(false)
    const triggerRef = useRef(null)
    const [position, setPosition] = useState({ top: 0, left: 0 })

    useEffect(() => {
        setRating(score)
        setComment(commentaire)
    }, [score, commentaire])

    const calculatePosition = () => {
        if (!triggerRef.current) return

        const triggerRect = triggerRef.current.getBoundingClientRect()
        const windowWidth = window.innerWidth
        const windowHeight = window.innerHeight
        const formWidth = 400
        const formHeight = 400

        let left = triggerRect.left - formWidth / 2 + triggerRect.width / 2
        let top = triggerRect.bottom + 10

        if (left + formWidth > windowWidth) {
            left = windowWidth - formWidth - 10
        } else if (left < 10) {
            left = 10
        }

        if (top + formHeight > windowHeight) {
            top = triggerRect.top - formHeight - 10
        }

        setPosition({
            top: Math.max(10, top) + window.scrollY,
            left: Math.max(10, left)
        })
    }

    const handleSubmit = async (e) => {
        e.preventDefault()
        setIsSubmitting(true)

        try {
            const evaluationData = {
                respondentUserId: getUserIdFromCookie(),
                comment,
                type: 'Module',
                filiereId: 0,
                moduleId: course.moduleId,
                score: rating,
            }

            const baseUrl = process.env.NEXT_PUBLIC_API_EVALUATION_URL
            const url = idEval
                ? `${baseUrl}/api/evaluations/updateEvaluation`
                : `${baseUrl}/api/evaluations/addEvaluation`

            const method = idEval ? "PUT" : "POST"

            const res = await fetch(url, {
                method,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
                },
                body: JSON.stringify(evaluationData),
            })

            if (!res.ok) throw new Error(await res.text())

            toast.success(idEval ? "Évaluation mise à jour" : "Évaluation enregistrée")
            if (onSuccess) onSuccess()
        } catch (error) {
            console.error("Erreur:", error)
            toast.error("Erreur lors de l'enregistrement")
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button
                    ref={triggerRef}
                    onClick={calculatePosition}
                    className="rounded-md bg-purple-600 px-3 py-1 text-xs text-white hover:bg-purple-700"
                    size="sm"
                >
                    {title}
                </Button>
            </DialogTrigger>

            <DialogContent
                className="bg-white shadow-xl w-[400px] p-6"
                style={{
                    position: 'fixed',
                    top: `${position.top}px`,
                    left: `${position.left}px`,
                    transform: 'none',
                    margin: 0,
                    borderRadius: '0.5rem'
                }}
            >
                <DialogHeader>
                    <DialogTitle className="text-purple-600 text-lg">
                        {course.moduleName}
                    </DialogTitle>
                    <DialogDescription className="text-sm text-gray-600">
                        Professeur: {course.teacherFullName}
                    </DialogDescription>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="mt-4 space-y-4">
                    <div className="space-y-2">
                        <label className="block text-sm font-medium text-gray-700">
                            Note
                        </label>
                        <div className="flex gap-1">
                            {[1, 2, 3, 4, 5].map((star) => (
                                <button
                                    key={star}
                                    type="button"
                                    onClick={() => setRating(star)}
                                    className={`text-3xl transition-colors ${star <= rating ? 'text-yellow-400' : 'text-gray-300'} hover:text-yellow-500`}
                                >
                                    ★
                                </button>
                            ))}
                        </div>
                    </div>

                    <div className="space-y-2">
                        <label className="block text-sm font-medium text-gray-700">
                            Commentaire
                        </label>
                        <Textarea
                            value={comment}
                            onChange={(e) => setComment(e.target.value)}
                            placeholder="Votre expérience avec ce module..."
                            className="min-h-[120px]"
                        />
                    </div>

                    <div className="flex justify-end gap-2 pt-2">
                        <Button
                            type="submit"
                            className="bg-purple-600 hover:bg-purple-700 px-4 py-2 text-sm"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? "Envoi..." : "Envoyer"}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    )
}