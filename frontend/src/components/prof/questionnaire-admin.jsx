"use client"

import { useState } from "react"
import { Plus, Type, List, Pencil, Trash2 } from "lucide-react"
import { Button } from "@/components/ui/button"
import {
    Table, TableBody, TableCell, TableHead, TableHeader, TableRow,
} from "@/components/ui/table"
import {
    Dialog, DialogContent, DialogHeader, DialogTitle,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"

const questionTypes = [
    { value: "ouverte", label: "Question ouverte", icon: Type },
    { value: "choix_multiple", label: "Choix multiple", icon: List }
]

const questionnaireTypes = [
    { value: "Interne", label: "Visiteurs internes" },
    { value: "Externe", label: "Visiteurs externes" }
]

const modulesList = [
    "Java", "Python", "Développement Web", "Base de données",
    "Réseaux", "Systèmes embarqués"
]

const fakeQuestionnaires = [
    {
        id: 1,
        title: "Évaluation du module Java",
        module: "Java",
        type: "Interne",
        questions: [
            {
                id: 1,
                question_text: "Définir une classe en Java",
                question_type: "ouverte",
                responses: ["Une classe est un modèle pour créer des objets"]
            },
            {
                id: 2,
                question_text: "Quel est le mot-clé pour hériter ?",
                question_type: "ouverte",
                responses: ["extends"]
            }
        ],
        status: true,
        created: "2024-03-15 09:15"
    },
    {
        id: 2,
        title: "Quiz Python",
        module: "Python",
        type: "Externe",
        questions: [
            {
                id: 1,
                question_text: "Différence entre list et tuple ?",
                question_type: "ouverte",
                responses: ["list est mutable", "tuple est immutable"]
            },
            {
                id: 2,
                question_text: "Python est-il typé dynamiquement ?",
                question_type: "choix_multiple",
                responses: ["Oui"]
            }
        ],
        status: false,
        created: "2024-04-02 14:00"
    },
    {
        id: 3,
        title: "Évaluation Réseaux",
        module: "Réseaux",
        type: "Interne",
        questions: [
            {
                id: 1,
                question_text: "Quelle est la couche 3 du modèle OSI ?",
                question_type: "ouverte",
                responses: ["La couche réseau","La couche application"]
            }
        ],
        status: true,
        created: "2024-04-20 10:45"
    }
]

export default function QuestionnairesManager() {
    const [questionnaires, setQuestionnaires] = useState(fakeQuestionnaires)
    const [currentQ, setCurrentQ] = useState(null)
    const [openDialog, setOpenDialog] = useState(false)
    const [openType, setOpenType] = useState('create')
    const [selectedQuestions, setSelectedQuestions] = useState([])
    const [questionnaireTitle, setQuestionnaireTitle] = useState("")

    const handleSubmit = (e) => {
        e.preventDefault()
        if (openType === 'create') {
            setQuestionnaires([...questionnaires, {
                ...currentQ,
                id: Date.now(),
                created: new Date().toLocaleString("fr-FR").replace(",", ""),
                questions: currentQ.questions || [],
                status: true
            }])
        } else {
            setQuestionnaires(questionnaires.map(q => q.id === currentQ.id ? currentQ : q))
        }
        setOpenDialog(false)
    }

    const deleteQuestionnaire = (id) => {
        setQuestionnaires(questionnaires.filter(q => q.id !== id))
    }

    const editQuestionnaire = (q) => {
        setCurrentQ(q)
        setOpenType('edit')
        setOpenDialog(true)
    }

    return (
        <div className="p-6">
            <div className="mb-6 flex items-center justify-between">
                <h1 className="text-2xl font-bold">Welcome to CPS</h1>
                <Button
                    onClick={() => {
                        setCurrentQ({ title: '', type: '', module: '', questions: [] })
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
                <Table>
                    <TableHeader className="bg-[#f8f9fa]">
                        <TableRow>
                            <TableHead>Titre</TableHead>
                            <TableHead>Module</TableHead>
                            <TableHead>Type</TableHead>
                            <TableHead>Questions</TableHead>
                            <TableHead>Statut</TableHead>
                            <TableHead>Date</TableHead>
                            <TableHead>Rapport</TableHead>
                            <TableHead>Actions</TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {questionnaires.map((q) => (
                            <TableRow key={q.id} onClick={() => {
                                setSelectedQuestions(q.questions)
                                setQuestionnaireTitle(q.title)
                            }} className="cursor-pointer">
                                <TableCell>{q.title}</TableCell>
                                <TableCell>{q.module}</TableCell>
                                <TableCell>{q.type}</TableCell>
                                <TableCell>{q.questions?.length || 0}</TableCell>
                                <TableCell>
                                    <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${q.status ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'}`}>{q.status ? "Terminé" : "En cours"}</span>
                                </TableCell>
                                <TableCell>{q.created}</TableCell>
                                <TableCell>
                                    <Button variant="outline" className="text-xs">Générer</Button>
                                </TableCell>
                                <TableCell className="flex gap-2">
                                    <Button type="button" size="icon" variant="ghost" onClick={(e) => { e.stopPropagation(); editQuestionnaire(q); }}>
                                        <Pencil className="h-4 w-4 text-blue-500" />
                                    </Button>
                                    <Button type="button" size="icon" variant="ghost" onClick={(e) => { e.stopPropagation(); deleteQuestionnaire(q.id); }}>
                                        <Trash2 className="h-4 w-4 text-red-500" />
                                    </Button>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </div>

            {selectedQuestions.length > 0 && (
                <div className="mt-6 p-4 border border-gray-300 bg-white rounded-md">
                    <h2 className="text-lg font-semibold mb-3">Questions : {questionnaireTitle}</h2>
                    <ul className="list-disc list-inside space-y-2">
                        {selectedQuestions.map((q, index) => (
                            <li key={index}>
                                <strong>Q{index + 1}:</strong> {q.question_text}<br />
                                {q.responses?.length > 0 && (
                                    <ul className="ml-6 list-disc text-sm text-gray-600">
                                        {q.responses.map((resp, i) => (
                                            <li key={i}>➡ {resp}</li>
                                        ))}
                                    </ul>
                                )}
                            </li>
                        ))}
                    </ul>
                </div>
            )}

            <Dialog open={openDialog} onOpenChange={setOpenDialog}>
                <DialogContent className="sm:max-w-[700px] bg-white max-h-screen overflow-y-auto">
                    <DialogHeader>
                        <DialogTitle>
                            {openType === 'create' ? 'Nouveau questionnaire' : 'Modifier questionnaire'}
                        </DialogTitle>
                    </DialogHeader>

                    <form onSubmit={handleSubmit} className="space-y-5">
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <Label>Titre *</Label>
                                <Input
                                    required
                                    value={currentQ?.title || ''}
                                    onChange={(e) => setCurrentQ({ ...currentQ, title: e.target.value })}
                                />
                            </div>
                            <div className="space-y-2">
                                <Label>Module *</Label>
                                <select
                                    required
                                    value={currentQ?.module || ''}
                                    onChange={(e) => setCurrentQ({ ...currentQ, module: e.target.value })}
                                    className="w-full rounded-md border-gray-300"
                                >
                                    <option value="">Sélectionner un module</option>
                                    {modulesList.map((mod) => (
                                        <option key={mod} value={mod}>{mod}</option>
                                    ))}
                                </select>
                            </div>
                            <div className="space-y-2 col-span-2">
                                <Label>Type *</Label>
                                <select
                                    required
                                    value={currentQ?.type || ''}
                                    onChange={(e) => setCurrentQ({ ...currentQ, type: e.target.value })}
                                    className="w-full rounded-md border-gray-300"
                                >
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
                                <div key={index} className="p-4 border rounded-md bg-gray-50">
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
                                </div>
                            ))}
                            <Button
                                type="button"
                                variant="outline"
                                className="w-full"
                                onClick={() => setCurrentQ({
                                    ...currentQ,
                                    questions: [...(currentQ?.questions || []), {
                                        id: Date.now(),
                                        question_text: '',
                                        question_type: 'ouverte',
                                        responses: []
                                    }]
                                })}
                            >
                                <Plus className="h-4 w-4 mr-2" />
                                Ajouter une question
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
        </div>
    )
}
