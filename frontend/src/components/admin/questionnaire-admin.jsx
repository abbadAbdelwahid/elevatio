"use client"

import { useState } from "react"
import { Plus, Pencil, Trash, Eye, Type, List } from "lucide-react"
import { Button } from "@/components/ui/button"
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog"
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
} from "@/components/ui/alert-dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Switch } from "@/components/ui/switch"

const questionTypes = [
    { value: "ouverte", label: "Question ouverte", icon: Type },
    { value: "choix_multiple", label: "Choix multiple", icon: List },
]

const questionnaireTypes = [
    { value: "Filière", label: "Pour la filière" },
    { value: "EDT", label: "Emploi du temps" },
    { value: "Visiteurs", label: "Visiteurs externes" },
]

export default function QuestionnairesManager({
                                                  questionnaires,
                                                  onSave,
                                                  onDelete,
                                                  onToggleStatus,
                                                  loading,
                                              }) {
    const [currentQ, setCurrentQ] = useState(null)
    const [openDialog, setOpenDialog] = useState(false)
    const [openType, setOpenType] = useState("create")
    const [viewingQuestions, setViewingQuestions] = useState(null)

    const handleSubmit = (e) => {
        e.preventDefault()
        const payload = {
            ...currentQ,
            created:
                openType === "create"
                    ? new Date().toISOString().split("T")[0]
                    : currentQ.created,
            questions: currentQ.questions || [],
        }
        onSave(payload)
        setOpenDialog(false)
    }

    const addQuestionOption = (questionIndex) => {
        const newQuestions = [...currentQ.questions]
        newQuestions[questionIndex].options = [
            ...(newQuestions[questionIndex].options || []),
            "",
        ]
        setCurrentQ({ ...currentQ, questions: newQuestions })
    }

    const handleOptionChange = (questionIndex, optionIndex, value) => {
        const newQuestions = [...currentQ.questions]
        newQuestions[questionIndex].options[optionIndex] = value
        setCurrentQ({ ...currentQ, questions: newQuestions })
    }

    // Fonction utilitaire pour afficher les réponses à choix multiple
    const renderMultipleChoiceAnswers = (question) => {
        if (!question.responses?.length) return <p className="text-gray-500 italic">Aucune réponse</p>
        return question.responses.map((resp, i) => {
            // resp.answer est un tableau d'indices choisis
            const answers = resp.answer
                .map((idx) => question.options?.[idx])
                .filter(Boolean)
                .join(", ")
            return (
                <p key={i} className="text-sm text-gray-700">
                    <strong>{resp.userId || `Utilisateur ${i + 1}`}:</strong> {answers || "Aucune sélection"}
                </p>
            )
        })
    }

    // Fonction utilitaire pour afficher les réponses ouvertes
    const renderOpenAnswers = (question) => {
        if (!question.responses?.length) return <p className="text-gray-500 italic">Aucune réponse</p>
        return question.responses.map((resp, i) => (
            <p key={i} className="text-sm text-gray-700">
                <strong>{resp.userId || `Utilisateur ${i + 1}`}:</strong> {resp.answer}
            </p>
        ))
    }

    return (
        <>
            <div className="flex items-center justify-end p-4">
                <Button
                    onClick={() => {
                        setCurrentQ({ questions: [], status: true })
                        setOpenType("create")
                        setOpenDialog(true)
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
                            <TableRow key={q.id} className="border-b border-gray-100 hover:bg-gray-50">
                                <TableCell className="px-4 py-3 font-medium text-gray-900">{q.title}</TableCell>

                                <TableCell className="px-4 py-3">
                  <span
                      className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${
                          q.type === "Filière"
                              ? "bg-purple-100 text-purple-800"
                              : q.type === "EDT"
                                  ? "bg-blue-100 text-blue-800"
                                  : "bg-green-100 text-green-800"
                      }`}
                  >
                    {q.type}
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
                                        onCheckedChange={(checked) => onToggleStatus(q.id, checked)}
                                    />
                                </TableCell>

                                <TableCell className="px-4 py-3 text-gray-600">{q.created}</TableCell>

                                <TableCell className="px-4 py-3 text-right">
                                    <div className="flex justify-end gap-2">
                                        <Button
                                            variant="ghost"
                                            size="sm"
                                            className="text-gray-400 hover:text-blue-600"
                                            onClick={() => {
                                                setCurrentQ(q)
                                                setOpenType("edit")
                                                setOpenDialog(true)
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
                                                        onClick={() => onDelete(q.id)}
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
                          question.question_type === "ouverte"
                              ? "bg-green-100 text-green-800"
                              : "bg-purple-100 text-purple-800"
                      }`}
                  >
                    {questionTypes.find((t) => t.value === question.question_type)?.label}
                  </span>
                                </div>
                                <p className="text-sm font-medium text-gray-900">{question.question_text}</p>

                                {/* Affichage des réponses selon le type */}
                                {question.question_type === "choix_multiple" && question.options?.length > 0 && (
                                    <>
                                        <p className="text-xs font-medium text-gray-500 mt-2">Options :</p>
                                        <ul className="list-disc pl-5 space-y-1">
                                            {question.options.map((opt, i) => (
                                                <li key={i} className="text-sm text-gray-700">
                                                    {opt}
                                                </li>
                                            ))}
                                        </ul>
                                        <p className="text-xs font-medium text-gray-500 mt-4">Réponses :</p>
                                        {renderMultipleChoiceAnswers(question)}
                                    </>
                                )}

                                {question.question_type === "ouverte" && (
                                    <>
                                        <p className="text-xs font-medium text-gray-500 mt-2">Réponses :</p>
                                        {renderOpenAnswers(question)}
                                    </>
                                )}
                            </div>
                        ))}
                    </div>
                </DialogContent>
            </Dialog>

            {/* Dialog créer/modifier (reste inchangé) */}
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
                                <Label className="text-sm font-medium text-gray-700">Type *</Label>
                                <select
                                    className="w-full rounded-md border-gray-300"
                                    value={currentQ?.type || ""}
                                    onChange={(e) => setCurrentQ({ ...currentQ, type: e.target.value })}
                                    required
                                >
                                    <option value="">Sélectionner un type</option>
                                    {questionnaireTypes.map((type) => (
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
                                            value={question.question_text}
                                            onChange={(e) => {
                                                const updated = [...currentQ.questions]
                                                updated[index].question_text = e.target.value
                                                setCurrentQ({ ...currentQ, questions: updated })
                                            }}
                                        />
                                        <select
                                            className="w-full rounded-md border-gray-300"
                                            value={question.question_type}
                                            onChange={(e) => {
                                                const updated = [...currentQ.questions]
                                                updated[index].question_type = e.target.value
                                                if (e.target.value === "choix_multiple" && !updated[index].options) {
                                                    updated[index].options = [""]
                                                }
                                                setCurrentQ({ ...currentQ, questions: updated })
                                            }}
                                            required
                                        >
                                            {questionTypes.map((t) => (
                                                <option key={t.value} value={t.value}>
                                                    {t.label}
                                                </option>
                                            ))}
                                        </select>

                                        {question.question_type === "choix_multiple" && (
                                            <div className="space-y-2">
                                                {question.options?.map((opt, optIdx) => (
                                                    <div key={optIdx} className="flex items-center gap-2">
                                                        <Input
                                                            required
                                                            value={opt}
                                                            onChange={(e) => handleOptionChange(index, optIdx, e.target.value)}
                                                            placeholder={`Option ${optIdx + 1}`}
                                                        />
                                                        <Button
                                                            type="button"
                                                            variant="ghost"
                                                            size="sm"
                                                            className="text-red-500"
                                                            onClick={() => {
                                                                const updated = [...currentQ.questions]
                                                                updated[index].options = updated[index].options.filter(
                                                                    (_, i) => i !== optIdx
                                                                )
                                                                setCurrentQ({ ...currentQ, questions: updated })
                                                            }}
                                                        >
                                                            <Trash className="h-4 w-4" />
                                                        </Button>
                                                    </div>
                                                ))}
                                                <Button
                                                    type="button"
                                                    variant="outline"
                                                    size="sm"
                                                    onClick={() => addQuestionOption(index)}
                                                >
                                                    <Plus className="h-4 w-4 mr-2" /> Ajouter une option
                                                </Button>
                                            </div>
                                        )}
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
                                                id: Date.now(),
                                                question_text: "",
                                                question_type: "ouverte",
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
    )
}
