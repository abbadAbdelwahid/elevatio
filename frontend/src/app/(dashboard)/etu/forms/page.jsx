'use client';

import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Input } from "@/components/ui/input";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { SearchHeader } from "@/components/layout/search-header";

// Fonction pour envoyer les réponses de l'étudiant
const sendAnswers = async (answers) => {
    const baseUrl = process.env.NEXT_PUBLIC_API_EVALUATION_URL;
    const token = localStorage.getItem("accessToken");

    try {
        const res = await fetch(`${baseUrl}/api/answers/addCleanRangeOfAnswers`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: token ? `Bearer ${token}` : "",
            },
            body: JSON.stringify(answers),
        });

        if (res.ok) {
            const data = await res.json();
            console.log("Réponses envoyées avec succès", data);
        } else {
            console.error("Erreur lors de l'envoi des réponses", await res.text());
        }
    } catch (err) {
        console.error("Erreur serveur:", err);
    }
};

export default function CoursesPage() {
    const [questionnaires, setQuestionnaires] = useState([]);
    const [answers, setAnswers] = useState([]); // Pour stocker les réponses des étudiants
    const [loading, setLoading] = useState(true);
    const [currentQuestionnaire, setCurrentQuestionnaire] = useState(null);
    const [openDialog, setOpenDialog] = useState(false);

    const baseUrl = process.env.NEXT_PUBLIC_API_EVALUATION_URL;
    const token = localStorage.getItem("accessToken");

    // Récupérer les questionnaires pour l'étudiant
    useEffect(() => {
        const fetchQuestionnaires = async () => {
            try {
                const res = await fetch(`${baseUrl}/api/questionnaires/all`, {
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: token ? `Bearer ${token}` : "",
                    },
                });

                if (res.ok) {
                    const data = await res.json();
                    setQuestionnaires(data); // Stocker les questionnaires dans l'état
                } else {
                    console.error("Erreur lors du fetch des questionnaires");
                }
            } catch (err) {
                console.error("Erreur serveur:", err);
            } finally {
                setLoading(false);
            }
        };

        fetchQuestionnaires();
    }, [baseUrl, token]);

    // Mettre à jour une réponse
    const handleAnswerChange = (questionId, value) => {
        setAnswers((prevAnswers) => {
            const updatedAnswers = prevAnswers.filter(
                (answer) => answer.questionId !== questionId
            );
            updatedAnswers.push({
                questionId,
                respondentUserId: "studentId", // Remplacer par l'ID de l'étudiant
                rawAnswer: value,
            });
            return updatedAnswers;
        });
    };

    // Soumettre les réponses de l'étudiant
    const handleSubmit = () => {
        sendAnswers(answers);
        setOpenDialog(false); // Fermer le dialogue après la soumission
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />

                {/* Affichage des questionnaires sous forme de tableau */}
                <div>
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>Titre</TableHead>
                                <TableHead>Type</TableHead>
                                <TableHead>Réponses</TableHead>
                                <TableHead>Soumettre les réponses</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {questionnaires.map((questionnaire) => (
                                <TableRow key={questionnaire.questionnaireId}>
                                    <TableCell>{questionnaire.title}</TableCell>
                                    <TableCell>{questionnaire.typeModuleFiliere}</TableCell>
                                    <TableCell>{questionnaire.questions.length}</TableCell>
                                    <TableCell>
                                        <Button
                                            onClick={() => {
                                                setCurrentQuestionnaire(questionnaire);
                                                setOpenDialog(true);
                                            }}
                                            className="bg-blue-600 text-white"
                                        >
                                            Soumettre les réponses
                                        </Button>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </div>
            </div>

            {/* Dialogue pour saisir les réponses */}
            <Dialog open={openDialog} onOpenChange={setOpenDialog}>
                <DialogContent className="sm:max-w-[700px] bg-white">
                    <DialogHeader>
                        <DialogTitle className="text-lg font-semibold text-gray-900">
                            Répondez aux questions - {currentQuestionnaire?.title}
                        </DialogTitle>
                    </DialogHeader>

                    <div className="space-y-4 max-h-[60vh] overflow-y-auto">
                        {currentQuestionnaire?.questions.map((question) => (
                            <div key={question.questionId} className="flex flex-col p-4 border rounded-lg bg-gray-50">
                                <label className="text-sm font-medium text-gray-700">{question.text}</label>
                                <Input
                                    type="text"
                                    placeholder="Votre réponse ici"
                                    onChange={(e) => handleAnswerChange(question.questionId, e.target.value)}
                                />
                            </div>
                        ))}
                    </div>

                    {/* Bouton pour soumettre les réponses */}
                    <div className="flex justify-end gap-3 mt-4">
                        <Button variant="outline" onClick={() => setOpenDialog(false)}>
                            Annuler
                        </Button>
                        <Button className="bg-blue-600 text-white" onClick={handleSubmit}>
                            Soumettre les réponses
                        </Button>
                    </div>
                </DialogContent>
            </Dialog>
        </div>
    );
}
