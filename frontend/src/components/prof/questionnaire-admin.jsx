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
import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "@/components/ui/alert-dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Switch } from "@/components/ui/switch"

const initialQuestionnaires = [
    {
        id: 1,
        title: "Évaluation du module Java",
        module: "Java",
        type: "Module",
        questions: [
            {
                id: 1,
                question_text: "Qu'est-ce qu'une classe en Java ?",
                question_type: "ouverte",
                responses: []
            },
            {
                id: 2,
                question_text: "Java est-il un langage compilé ou interprété ?",
                question_type: "choix_multiple",
                options: ["Compilé", "Interprété", "Les deux"],
                responses: []
            }
        ],
        status: true,
        created: "2024-03-15"
    }
]

const questionTypes = [
    { value: "ouverte", label: "Question ouverte", icon: Type },
    { value: "choix_multiple", label: "Choix multiple", icon: List }
]

const questionnaireTypes = [
    { value: "Module", label: "Pour le module" },
    { value: "Externe", label: "Visiteurs externes" }
]

const modulesList = [
    "Java",
    "Python",
    "Développement Web",
    "Base de données",
    "Réseaux",
    "Systèmes embarqués"
]

export default function QuestionnairesManager() {
    const [questionnaires, setQuestionnaires] = useState(initialQuestionnaires)
    const [currentQ, setCurrentQ] = useState(null)
    const [openDialog, setOpenDialog] = useState(false)
    const [openType, setOpenType] = useState('create')
    const [viewingQuestions, setViewingQuestions] = useState(null)

    const handleSubmit = (e) => {
        e.preventDefault()
        if (openType === 'create') {
            setQuestionnaires([...questionnaires, {
                ...currentQ,
                id: Date.now(),
                created: new Date().toISOString().split('T')[0],
                questions: currentQ.questions || []
            }])
        } else {
            setQuestionnaires(questionnaires.map(q => q.id === currentQ.id ? currentQ : q))
        }
        setOpenDialog(false)
    }

    const handleDelete = (id) => {
        setQuestionnaires(questionnaires.filter(q => q.id !== id))
    }

    const addQuestionOption = (questionIndex) => {
        const newQuestions = [...currentQ.questions]
        newQuestions[questionIndex].options = [...(newQuestions[questionIndex].options || []), ""]
        setCurrentQ({ ...currentQ, questions: newQuestions })
    }

    const handleOptionChange = (questionIndex, optionIndex, value) => {
        const newQuestions = [...currentQ.questions]
        newQuestions[questionIndex].options[optionIndex] = value
        setCurrentQ({ ...currentQ, questions: newQuestions })
    }

    return (
        <div className="p-6">
            <div className="mb-6 flex items-center justify-between">
                <h1 className="text-2xl font-bold">Welcome to CPS</h1>
                <Button
                    onClick={() => {
                        setCurrentQ({ questions: [], module: "", type: "" })
                        setOpenType('create')
                        setOpenDialog(true)
                    }}
                    className="bg-[#7F56D9] hover:bg-[#3a1a4a] text-white h-10"
                >
                    <Plus className="h-4 w-4 mr-2" />
                    Nouveau questionnaire
                </Button>
            </div>

            <div className="rounded-lg border border-gray-200 bg-white shadow-sm">
                {/* table content (unchanged) */}
            </div>

            {openDialog && (
                <Dialog open={openDialog} onOpenChange={setOpenDialog}>
                    <DialogContent className="sm:max-w-[700px] bg-white max-h-screen overflow-y-auto">
                        <DialogHeader>
                            <DialogTitle>{openType === 'create' ? 'Nouveau questionnaire' : 'Modifier questionnaire'}</DialogTitle>
                        </DialogHeader>

                        <form onSubmit={handleSubmit} className="space-y-5">
                            <div className="grid grid-cols-2 gap-4">
                                <div className="space-y-2">
                                    <Label>Titre *</Label>
                                    <Input required value={currentQ?.title || ''} onChange={(e) => setCurrentQ({ ...currentQ, title: e.target.value })} />
                                </div>
                                <div className="space-y-2">
                                    <Label>Module *</Label>
                                    <select required value={currentQ?.module || ''} onChange={(e) => setCurrentQ({ ...currentQ, module: e.target.value })} className="w-full rounded-md border-gray-300">
                                        <option value="">Sélectionner un module</option>
                                        {modulesList.map((mod) => (
                                            <option key={mod} value={mod}>{mod}</option>
                                        ))}
                                    </select>
                                </div>
                                <div className="space-y-2 col-span-2">
                                    <Label>Type *</Label>
                                    <select required value={currentQ?.type || ''} onChange={(e) => setCurrentQ({ ...currentQ, type: e.target.value })} className="w-full rounded-md border-gray-300">
                                        <option value="">Sélectionner un type</option>
                                        {questionnaireTypes.map((type) => (
                                            <option key={type.value} value={type.value}>{type.label}</option>
                                        ))}
                                    </select>
                                </div>
                            </div>

                            <div className="space-y-4">
                                <Label>Questions *</Label>
                                {(currentQ?.questions || []).map((question, index) => (
                                    <div key={question.id || index} className="p-4 border rounded-md bg-gray-50">
                                        <Label>Question {index + 1}</Label>
                                        <Input
                                            required
                                            placeholder="Entrez la question"
                                            value={question.question_text}
                                            onChange={(e) => {
                                                const updated = [...currentQ.questions]
                                                updated[index].question_text = e.target.value
                                                setCurrentQ({ ...currentQ, questions: updated })
                                            }}
                                        />
                                        <div className="mt-2">
                                            <select
                                                required
                                                className="w-full rounded-md border-gray-300"
                                                value={question.question_type}
                                                onChange={(e) => {
                                                    const updated = [...currentQ.questions]
                                                    updated[index].question_type = e.target.value
                                                    if (e.target.value === 'choix_multiple' && !updated[index].options) {
                                                        updated[index].options = ['']
                                                    }
                                                    setCurrentQ({ ...currentQ, questions: updated })
                                                }}
                                            >
                                                {questionTypes.map(type => (
                                                    <option key={type.value} value={type.value}>{type.label}</option>
                                                ))}
                                            </select>
                                        </div>
                                        {question.question_type === 'choix_multiple' && (
                                            <div className="space-y-2 mt-2">
                                                {question.options?.map((opt, i) => (
                                                    <div key={i} className="flex gap-2">
                                                        <Input
                                                            value={opt}
                                                            onChange={(e) => handleOptionChange(index, i, e.target.value)}
                                                            placeholder={`Option ${i + 1}`}
                                                        />
                                                        <Button type="button" onClick={() => {
                                                            const updated = [...currentQ.questions]
                                                            updated[index].options = updated[index].options.filter((_, j) => j !== i)
                                                            setCurrentQ({ ...currentQ, questions: updated })
                                                        }}>❌</Button>
                                                    </div>
                                                ))}
                                                <Button type="button" onClick={() => addQuestionOption(index)}>
                                                    + Ajouter une option
                                                </Button>
                                            </div>
                                        )}
                                        <div className="mt-2 text-sm text-gray-600">
                                            Réponses : {question.responses?.length || 0}
                                        </div>
                                    </div>
                                ))}
                                <Button
                                    type="button"
                                    variant="outline"
                                    className="w-full"
                                    onClick={() => setCurrentQ({
                                        ...currentQ,
                                        questions: [...(currentQ?.questions || []), { id: Date.now(), question_text: '', question_type: 'ouverte', responses: [] }]
                                    })}
                                >
                                    <Plus className="h-4 w-4 mr-2" /> Ajouter une question
                                </Button>
                            </div>

                            <div className="flex justify-end gap-3">
                                <Button type="button" variant="outline" onClick={() => setOpenDialog(false)}>Annuler</Button>
                                <Button type="submit" className="bg-[#4a2a5a] hover:bg-[#3a1a4a] text-white">
                                    {openType === 'create' ? 'Créer' : 'Enregistrer'}
                                </Button>
                            </div>
                        </form>
                    </DialogContent>
                </Dialog>
            )}
        </div>
    )
}
