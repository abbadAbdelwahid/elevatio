'use client'

import { useState, useRef } from "react"
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
import {getUserIdFromCookie} from "@/lib/utils";

export function DialogDemo({ title, course ,score,commentaire,idEval }) {

    const [rating, setRating] = useState(score)

    const [comment, setComment] = useState(commentaire)
   console.log('idEval: ',title,score,commentaire,rating)
    const [successMessage, setSuccessMessage] = useState("") // Nouveau state pour message de succès
    const [isSubmitting, setIsSubmitting] = useState(false) // Nouveau state pour la soumission en cours
    const triggerRef = useRef(null)
    const contentRef = useRef(null)
    const [position, setPosition] = useState({ top: 0, left: 0 })

    const calculatePosition = () => {
        if (!triggerRef.current) return

        const triggerRect = triggerRef.current.getBoundingClientRect()
        const windowWidth = window.innerWidth
        const windowHeight = window.innerHeight
        const formWidth = 400
        const formHeight = 320

        // Position horizontale
        let leftPosition = triggerRect.right + 10 // 10px de marge à droite du bouton
        if (leftPosition + formWidth > windowWidth) {
            leftPosition = triggerRect.left - formWidth - 10 // 10px de marge à gauche du bouton
        }

        // Position verticale
        let topPosition = triggerRect.top
        const spaceBelow = windowHeight - triggerRect.bottom
        if (spaceBelow < formHeight) {
            topPosition = triggerRect.top - formHeight
        }

        // Contraintes finales
        topPosition = Math.max(10, Math.min(topPosition, windowHeight - formHeight - 10))
        leftPosition = Math.max(10, Math.min(leftPosition, windowWidth - formWidth - 10))

        setPosition({
            top: topPosition + window.scrollY, // Ajout du scroll vertical
            left: leftPosition
        })
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);

        const evaluationData = {
            respondentUserId: getUserIdFromCookie(), // ID de l'utilisateur
            comment: comment, // Commentaire saisi par l'utilisateur
            type: 'Module',  // Type d'évaluation (peut être "Module" ou "Filiere")
            filiereId: 0,    // ID de la filière (mettre à 0 si non utilisé)
            moduleId: course.moduleId, // ID du module
            score: rating,  // Score de l'évaluation (basé sur le rating)
        };

        try {
            const apiUrl = `${process.env.NEXT_PUBLIC_API_EVALUATION_URL}/api/evaluations`;
            const url = course.score>0 ? `${apiUrl}/updateEvaluation/${idEval}` : `${apiUrl}/addEvaluation`;

            const method = course.score >0 ? "PUT" : "POST"; // PUT pour la mise à jour, POST pour l'ajout

            const res = await fetch(url, {
                method: method,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("accessToken")}`, // Authentification
                },
                body: JSON.stringify(evaluationData),
            });

            if (res.ok) {
                setSuccessMessage(course.score ? "Évaluation modifiée avec succès !" : "Évaluation soumise avec succès !");
                setTimeout(() => {
                    setSuccessMessage("");
                    setIsSubmitting(false);
                }, 3000);
            } else {
                console.error("Erreur lors de la soumission de l'évaluation");
                setIsSubmitting(false);
            }
        } catch (error) {
            console.error("Erreur réseau:", error);
            setIsSubmitting(false);
        }
    };



    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button
                    ref={triggerRef}
                    onClick={calculatePosition}
                    className="rounded-md bg-purple-600 px-3 py-1 text-xs text-white hover:bg-purple-700"
                >
                    {title}
                </Button>
            </DialogTrigger>

            <DialogContent
                ref={contentRef}
                className="bg-white shadow-xl w-[400px]"
                style={{
                    position: 'fixed',
                    top: position.top,
                    left: position.left,
                    maxHeight: '90vh',
                    overflowY: 'auto',
                    transform: 'none',
                    margin: 0
                }}
            >
                <DialogHeader>
                    <DialogTitle className="text-purple-600">
                        Évaluer {course.moduleName}
                    </DialogTitle>
                    <DialogDescription className="text-sm">
                        {course.teacherFullName}
                    </DialogDescription>
                </DialogHeader>

                {successMessage && (
                    <div className="bg-green-100 text-green-800 p-2 mb-4 rounded">
                        {successMessage}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4 pt-2">
                    <div className="flex gap-1">
                        {[1, 2, 3, 4, 5].map((star) => (
                            <button
                                key={star}
                                type="button"
                                onClick={() => setRating(star)}
                                className={`text-2xl ${star <= rating ? 'text-yellow-400' : 'text-gray-300'} hover:text-yellow-500`}
                            >
                                ★
                            </button>
                        ))}
                    </div>

                    <Textarea
                        value={comment}
                        onChange={(e) => setComment(e.target.value)}
                        placeholder="Partagez votre expérience..."
                        className="min-h-[100px]"
                    />

                    <div className="flex justify-end gap-2">
                        <Button
                            type="submit"
                            className="bg-purple-600 hover:bg-purple-700 px-4 py-2 text-sm"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? "Envoi en cours..." : "Soumettre"}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    )
}
