"use client";

import { useState, useEffect } from "react";
import { Pencil, RotateCcw } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
    Table, TableBody, TableCell, TableHead, TableHeader, TableRow
} from "@/components/ui/table";
import {
    Dialog, DialogContent, DialogHeader, DialogTitle
} from "@/components/ui/dialog";
import { getUserIdFromCookie } from "@/lib/utils";
import { toast } from "sonner";

export default function QuestionnairesManager({ questionnaires = [], loading, onRefresh }) {
    const [viewingQuestions, setViewingQuestions] = useState(null);
    const [responses, setResponses] = useState({});
    const [existingAnswers, setExistingAnswers] = useState({});
    const [filterType, setFilterType] = useState("");
    const [userAnswers, setUserAnswers] = useState([]);

    useEffect(() => {
        const fetchUserAnswers = async () => {
            try {
                const res = await fetch(
                    `${process.env.NEXT_PUBLIC_API_EVALUATION_URL}/api/answers/getAnswersByRespondentId/${getUserIdFromCookie()}`
                );
                if (!res.ok) throw new Error("Erreur récupération réponses");
                setUserAnswers(await res.json());
            } catch (err) {
                console.error("Erreur chargement réponses :", err);
                toast.error("Erreur lors du chargement des réponses");
            }
        };

        if (questionnaires.length > 0) fetchUserAnswers();
    }, [questionnaires]);

    const handleInputChange = (questionId, value) => {
        setResponses(prev => ({ ...prev, [questionId]: value }));
    };

    const handleOpenQuestionnaire = questionnaire => {
        const preFilled = {};
        const preAnswers = {};
        const respondentId = questionnaire.creatorUserId;

        questionnaire.questions.forEach(q => {
            const ans = userAnswers
                .filter(a => a.questionId === q.questionId && a.respondentUserId === respondentId)
                .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))[0];

            if (ans) {
                preFilled[q.questionId] = ans.rawAnswer;
                preAnswers[q.questionId] = ans;
            }
        });

        setResponses(preFilled);
        setExistingAnswers(preAnswers);
        setViewingQuestions(questionnaire);
    };

    const handleSubmitResponses = async e => {
        e.preventDefault();
        if (!viewingQuestions) return;

        const respondentUserId = viewingQuestions.creatorUserId;
        const promises = [];

        try {
            for (const q of viewingQuestions.questions) {
                const rawAnswer = responses[q.questionId] || "";
                const base = {
                    questionId: q.questionId,
                    respondentUserId,
                    rawAnswer,
                    ratingAnswer: 5
                };

                const existing = existingAnswers[q.questionId];
                const url = existing
                    ? `${process.env.NEXT_PUBLIC_API_EVALUATION_URL}/api/answers/updateCleanAnswer`
                    : `${process.env.NEXT_PUBLIC_API_EVALUATION_URL}/api/answers/addCleanRangeOfAnswers`;

                const method = existing ? "PUT" : "POST";

                const body = existing
                    ? { ...base, answerId: existing.answerId, createdAt: new Date().toISOString() }
                    : [base];

                promises.push(
                    fetch(url, {
                        method,
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(body)
                    })
                );
            }

            await Promise.all(promises);
            toast.success("Réponses enregistrées avec succès !");
            setViewingQuestions(null);
            setResponses({});
            setExistingAnswers({});
            onRefresh(); // Déclenche le rafraîchissement des données parentes
        } catch (err) {
            console.error("Erreur soumission:", err);
            toast.error("Erreur lors de l'enregistrement");
        }
    };

    const filteredQuestionnaires = filterType
        ? questionnaires.filter(q => q.typeModuleFiliere === filterType)
        : questionnaires;

    const uniqueTypes = [...new Set(questionnaires.map(q => q.typeModuleFiliere))];

    return (
        <>
            <div className="flex items-center justify-start p-4 gap-4 rounded-lg border border-gray-200 shadow-sm">
                <label>Filtrer par type :</label>
                <select
                    className="border rounded p-1 rounded-lg border border-gray-200 bg-white shadow-sm"
                    value={filterType}
                    onChange={e => setFilterType(e.target.value)}
                >
                    <option value="">Tous les types</option>
                    {uniqueTypes.map((type, i) => (
                        <option key={i} value={type}>{type}</option>
                    ))}
                </select>
            </div>

            <div className="rounded-lg border border-gray-200 bg-white shadow-sm">
                <Table>
                    <TableHeader className="bg-[#f8f9fa]">
                        <TableRow className="border-b border-gray-200">
                            <TableHead>Titre</TableHead>
                            <TableHead>Type</TableHead>
                            <TableHead>Date création</TableHead>
                            <TableHead>Questions</TableHead>
                            <TableHead className="text-right">Actions</TableHead> {/* Fixed closing tag */}
                        </TableRow>
                    </TableHeader>

                    <TableBody>
                        {[...filteredQuestionnaires]
                            .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
                            .map(q => {
                                const hasAnswered = q.questions.some(qst =>
                                    userAnswers.some(ans => ans.questionId === qst.questionId)
                                );
                                return (
                                    <TableRow key={q.questionnaireId} className="border-b border-gray-100 hover:bg-gray-50">
                                        <TableCell>{q.title}</TableCell>
                                        <TableCell>
                                            <span className={`inline-flex px-2.5 py-0.5 text-xs font-medium ${
                                                q.typeModuleFiliere === "Filiere" ? "bg-purple-100 text-purple-800" :
                                                    q.typeModuleFiliere === "Module" ? "bg-blue-100 text-blue-800" :
                                                        "bg-green-100 text-green-800"
                                            }`}>
                                                {q.typeModuleFiliere}
                                            </span>
                                        </TableCell>
                                        <TableCell>{new Date(q.createdAt).toLocaleDateString()}</TableCell>
                                        <TableCell>{q.questions.length}</TableCell>
                                        <TableCell className="text-right">
                                            <Button
                                                variant="ghost"
                                                size="sm"
                                                onClick={() => handleOpenQuestionnaire(q)}
                                            >
                                                {hasAnswered ? (
                                                    <>Mettre à jour <RotateCcw className="h-4 w-4 ml-1 text-green-600" /></>
                                                ) : (
                                                    <>Remplir <Pencil className="h-4 w-4 ml-1 text-blue-600" /></>
                                                )}
                                            </Button>
                                        </TableCell>
                                    </TableRow>
                                );
                            })}
                    </TableBody>
                </Table>
            </div>

            <Dialog open={!!viewingQuestions} onOpenChange={() => setViewingQuestions(null)}>
                <DialogContent className="sm:max-w-[700px] bg-white">
                    <DialogHeader>
                        <DialogTitle>Répondre au questionnaire – {viewingQuestions?.title}</DialogTitle>
                    </DialogHeader>
                    <form onSubmit={handleSubmitResponses}>
                        <div className="space-y-4 max-h-[60vh] overflow-y-auto">
                            {viewingQuestions?.questions.map(q => (
                                <div key={q.questionId} className="p-4 border rounded-lg bg-gray-50">
                                    <div className="mb-2 text-sm font-medium">{q.text}</div>
                                    <input
                                        type="text"
                                        className="w-full p-2 border rounded"
                                        placeholder="Répondre ici"
                                        value={responses[q.questionId] || ""}
                                        onChange={e => handleInputChange(q.questionId, e.target.value)}
                                    />
                                </div>
                            ))}
                        </div>
                        <Button type="submit" className="bg-[#4a2a5a] text-white mt-4">
                            {Object.keys(existingAnswers).length > 0 ? "Mettre à jour" : "Envoyer"}
                        </Button>
                    </form>
                </DialogContent>
            </Dialog>
        </>
);
}