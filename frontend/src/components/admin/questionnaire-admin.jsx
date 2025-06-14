"use client";

import { useState } from "react";
import { Plus, Pencil, Trash, Eye, Type, List } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog";
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";

// Définir les types pour les questionnaires
const questionnaireTypesInternalExternal = [
    { value: "Internal", label: "Internal" },
    { value: "External", label: "External" },
];

const questionnaireTypesModuleFiliere = [
    { value: "Filiere", label: "Filière" },
    { value: "EDT", label: "Emploi du temps" },
];

export default function QuestionnairesManager({
                                                  questionnaires,
                                                  onSave,
                                                  onDelete,
                                                  onToggleStatus,
                                                  loading,
                                              }) {
    const [currentQ, setCurrentQ] = useState(null);
    const [openDialog, setOpenDialog] = useState(false);
    const [openType, setOpenType] = useState("create");
    const [viewingQuestions, setViewingQuestions] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const payload = {
            ...currentQ,
            created:
                openType === "create"
                    ? new Date().toISOString().split("T")[0]
                    : currentQ.created,
            questions: currentQ.questions || [],
        };

        await onSave(payload); // Assurez-vous d'appeler la fonction pour la création ou la mise à jour
        setOpenDialog(false);
    };

    // Fonction utilitaire pour afficher les réponses ouvertes
    const renderOpenAnswers = (question) => {
        if (!question.answers?.length) {
            return <p className="text-gray-500 italic">Aucune réponse</p>;
        }

        return question.answers.map((resp, i) => (
            <p key={i} className="text-sm text-gray-700">
                <strong>{resp.respondentUserId || `Utilisateur ${i + 1}`}:</strong> {resp.rawAnswer || "Aucune réponse"}
            </p>
        ));
    };

    return (
        <>
            <div className="flex items-center justify-end p-4">
                <Button
                    onClick={() => {
                        setCurrentQ({ questions: [], status: true });
                        setOpenType("create");
                        setOpenDialog(true);
                    }}
                    className="bg-[#7F56D9] hover:bg-[#3a1a4a] text-white h-10"
                >
                    <Plus className="h-4 w-4 mr-2" />
                    Nouveau questionnaire
                </Button>
            </div>

            <div className="rounded-lg border border-gray-200 bg-white shadow-sm">
                <Table>
                    <TableHeader className="bg-[#f8f9fa]">
                        <TableRow className="border-b border-gray-200">
                            <TableHead className="px-4 py-3">Titre</TableHead>
                            <TableHead className="px-4 py-3">Type</TableHead>
                            <TableHead className="px-4 py-3">Questions</TableHead>
                            <TableHead className="px-4 py-3">Statut</TableHead>
                            <TableHead className="px-4 py-3">Date création</TableHead>
                            <TableHead className="px-4 py-3 text-right">Actions</TableHead>
                        </TableRow>
                    </TableHeader>

                    <TableBody>
                        {questionnaires.map((q) => (
                            <TableRow key={q.questionnaireId} className="border-b border-gray-100 hover:bg-gray-50">
                                <TableCell className="px-4 py-3 font-medium text-gray-900">{q.title}</TableCell>

                                <TableCell className="px-4 py-3">
                  <span
                      className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${
                          q.typeModuleFiliere === "Filiere"
                              ? "bg-purple-100 text-purple-800"
                              : q.typeModuleFiliere === "EDT"
                                  ? "bg-blue-100 text-blue-800"
                                  : "bg-green-100 text-green-800"
                      }`}
                  >
                    {q.typeModuleFiliere}
                  </span>
                                </TableCell>

                                <TableCell className="px-4 py-3">
                                    <div className="flex items-center gap-2">
                                        <span className="text-gray-600">{q.questions.length}</span>
                                        <Button
                                            variant="ghost"
                                            size="sm"
                                            className="text-gray-400 hover:text-blue-600"
                                            onClick={() => setViewingQuestions(q)}
                                        >
                                            <Eye className="h-4 w-4" />
                                        </Button>
                                    </div>
                                </TableCell>

                                <TableCell className="px-4 py-3">
                                    <Switch
                                        checked={q.status}
                                        onCheckedChange={(checked) => onToggleStatus(q.questionnaireId, checked)}
                                    />
                                </TableCell>

                                <TableCell className="px-4 py-3 text-gray-600">{new Date(q.createdAt).toLocaleDateString()}</TableCell>

                                <TableCell className="px-4 py-3 text-right">
                                    <div className="flex justify-end gap-2">
                                        <Button
                                            variant="ghost"
                                            size="sm"
                                            className="text-gray-400 hover:text-blue-600"
                                            onClick={() => {
                                                setCurrentQ(q);
                                                setOpenType("edit");
                                                setOpenDialog(true);
                                            }}
                                        >
                                            <Pencil className="h-4 w-4" />
                                        </Button>

                                        <AlertDialog>
                                            <AlertDialogTrigger asChild>
                                                <Button
                                                    variant="ghost"
                                                    size="sm"
                                                    className="text-gray-400 hover:text-red-600"
                                                >
                                                    <Trash className="h-4 w-4" />
                                                </Button>
                                            </AlertDialogTrigger>
                                            <AlertDialogContent>
                                                <AlertDialogHeader>
                                                    <AlertDialogTitle>Confirmer la suppression</AlertDialogTitle>
                                                    <AlertDialogDescription>
                                                        Êtes-vous sûr de vouloir supprimer "{q.title}" ?
                                                    </AlertDialogDescription>
                                                </AlertDialogHeader>
                                                <AlertDialogFooter>
                                                    <AlertDialogCancel>Annuler</AlertDialogCancel>
                                                    <AlertDialogAction
                                                        onClick={() => onDelete(q.questionnaireId)}
                                                        className="bg-red-600 hover:bg-red-700"
                                                    >
                                                        Confirmer
                                                    </AlertDialogAction>
                                                </AlertDialogFooter>
                                            </AlertDialogContent>
                                        </AlertDialog>
                                    </div>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </div>

            {/* Visualiser questions + réponses */}
            <Dialog open={!!viewingQuestions} onOpenChange={() => setViewingQuestions(null)}>
                <DialogContent className="sm:max-w-[700px] bg-white">
                    <DialogHeader>
                        <DialogTitle className="text-lg font-semibold text-gray-900">
                            Détail des questions - {viewingQuestions?.title}
                        </DialogTitle>
                    </DialogHeader>
                    <div className="space-y-4 max-h-[60vh] overflow-y-auto">
                        {viewingQuestions?.questions.map((question, index) => (
                            <div key={index} className="p-4 border rounded-lg bg-gray-50">
                                <div className="flex items-center gap-2 mb-2">
                  <span
                      className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${
                          question.statName === "MedianRating"
                              ? "bg-green-100 text-green-800"
                              : "bg-purple-100 text-purple-800"
                      }`}
                  >
                    {question.text}
                  </span>
                                </div>

                                {/* Affichage des réponses */}
                                <div>
                                    <p className="text-xs font-medium text-gray-500 mt-2">Réponses :</p>
                                    {renderOpenAnswers(question)} {/* Affichage des réponses ouvertes */}
                                </div>
                            </div>
                        ))}
                    </div>
                </DialogContent>
            </Dialog>

            {/* Dialog créer/modifier */}
            <Dialog open={openDialog} onOpenChange={setOpenDialog}>
                <DialogContent className="sm:max-w-[700px] bg-white max-h-screen overflow-y-auto mb-20">
                    <DialogHeader>
                        <DialogTitle className="text-lg font-semibold text-gray-900">
                            {openType === "create" ? "Nouveau questionnaire" : "Modifier questionnaire"}
                        </DialogTitle>
                    </DialogHeader>
                    <form onSubmit={handleSubmit} className="space-y-5">
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <Label className="text-sm font-medium text-gray-700">Titre *</Label>
                                <Input
                                    required
                                    value={currentQ?.title || ""}
                                    onChange={(e) => setCurrentQ({ ...currentQ, title: e.target.value })}
                                />
                            </div>
                            <div className="space-y-2">
                                <Label className="text-sm font-medium text-gray-700">Type (Internal / External) *</Label>
                                <select
                                    className="w-full rounded-md border-gray-300"
                                    value={currentQ?.typeInternalExternal || ""}
                                    onChange={(e) => setCurrentQ({ ...currentQ, typeInternalExternal: e.target.value })}
                                    required
                                >
                                    <option value="">Sélectionner un type</option>
                                    {questionnaireTypesInternalExternal.map((type) => (
                                        <option key={type.value} value={type.value}>
                                            {type.label}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        </div>

                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <Label className="text-sm font-medium text-gray-700">Type (EDT / Filiere) *</Label>
                                <select
                                    className="w-full rounded-md border-gray-300"
                                    value={currentQ?.typeModuleFiliere || ""}
                                    onChange={(e) => setCurrentQ({ ...currentQ, typeModuleFiliere: e.target.value })}
                                    required
                                >
                                    <option value="">Sélectionner un type</option>
                                    {questionnaireTypesModuleFiliere.map((type) => (
                                        <option key={type.value} value={type.value}>
                                            {type.label}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        </div>

                        {/* Questions */}
                        <div className="space-y-4">
                            <Label className="text-sm font-medium text-gray-700">Questions *</Label>
                            {(currentQ?.questions || []).map((question, index) => (
                                <div key={index} className="p-4 border rounded-lg bg-white">
                                    <div className="space-y-2">
                                        <Input
                                            required
                                            placeholder={`Question ${index + 1}`}
                                            value={question.text}
                                            onChange={(e) => {
                                                const updated = [...currentQ.questions];
                                                updated[index].text = e.target.value;
                                                setCurrentQ({ ...currentQ, questions: updated });
                                            }}
                                        />
                                    </div>
                                </div>
                            ))}
                            <Button
                                type="button"
                                variant="outline"
                                className="w-full"
                                onClick={() =>
                                    setCurrentQ({
                                        ...currentQ,
                                        questions: [
                                            ...(currentQ?.questions || []),
                                            {
                                                questionId: Date.now(),
                                                text: "",
                                            },
                                        ],
                                    })
                                }
                            >
                                <Plus className="h-4 w-4 mr-2" />
                                Ajouter une question
                            </Button>
                        </div>

                        <div className="flex justify-end gap-3">
                            <Button type="button" variant="outline" onClick={() => setOpenDialog(false)}>
                                Annuler
                            </Button>
                            <Button type="submit" className="bg-[#4a2a5a] hover:bg-[#3a1a4a] text-white">
                                {openType === "create" ? "Créer" : "Enregistrer"}
                            </Button>
                        </div>
                    </form>
                </DialogContent>
            </Dialog>
        </>
    );
}
