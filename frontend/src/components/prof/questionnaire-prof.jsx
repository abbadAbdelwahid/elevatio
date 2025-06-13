"use client";

import {useEffect, useState} from "react";
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
import {getUserIdFromCookie} from "@/lib/utils";


export default function QuestionnairesManager({
                                                  questionnaires,
                                                  onSave,
                                                  onDelete,
                                                  onToggleStatus,
                                                  loading,
                                              }) {
    console.log(questionnaires);
    const [currentQ, setCurrentQ] = useState(null);
    const [openDialog, setOpenDialog] = useState(false);
    const [openType, setOpenType] = useState("create");
    const [viewingQuestions, setViewingQuestions] = useState(null);
    const [modules, setModules] = useState(null);

    useEffect(() => {
        const fetchModules = async () => {
            try {
                const token =  localStorage.getItem("accessToken")
                const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL
                const res = await fetch(`${baseUrl}/api/Module/teacher/${getUserIdFromCookie()}`, {
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: token ? `Bearer ${token}` : "",
                    },
                });

                if (res.ok) {
                    const data = await res.json();
                    console.log(data);
                    setModules(data); // Sauvegarde les questionnaires récupérés
                } else {
                    console.error("Erreur lors du fetch initial");
                }
            } catch (err) {
                console.error("Erreur serveur:", err);
            }
        };

        fetchModules();
    }, []);

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

    const generatePdf = async (idModule,nameModule) => {

        console.log(idModule,nameModule);
        try {
            if (typeof window === "undefined") return;

            // ✅ dynamically load html2pdf ONLY on client
            const html2pdf = (await import('html2pdf.js')).default;
            const res = await fetch(`${process.env.NEXT_PUBLIC_API_ANALYTICS_URL}/api/ModuleReport/generateHtml/${idModule}`)
            const html = await res.text()

            const container = document.createElement("div")
            container.innerHTML = html
            document.body.appendChild(container)

            const datahtml= await html2pdf()
                .set({
                    margin: 0.5,
                    filename: `Rapport_${nameModule}.pdf`,
                    image: { type: "jpeg", quality: 0.98 },
                    html2canvas: { scale: 2 },
                    jsPDF: { unit: "in", format: "a4", orientation: "portrait" },
                })
                .from(container)
                .save()
            console.log(datahtml);
            document.body.removeChild(container)
        } catch (error) {
            console.error("Erreur PDF :", error)
            alert("❌ Impossible de générer le rapport PDF.")
        }
    }
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
                            <TableHead className="px-4 py-3">Date création</TableHead>
                            <TableHead className="px-4 py-3">Rapport</TableHead>
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

                                <TableCell className="px-4 py-3 text-gray-600">{new Date(q.createdAt).toLocaleDateString()}</TableCell>
                                <TableCell>
                                    <Button onClick={()=>generatePdf(q.moduleId,'test')}   variant="outline" className="text-xs" >Générer</Button>
                                </TableCell>

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
                                    onChange={(e) => setCurrentQ({ ...currentQ, title: e.target.value,typeModuleFiliere:'Module' })}
                                />
                            </div>
                            {/*<div className="space-y-2">*/}
                            {/*    <select className="w-full rounded-md border-gray-300" >*/}
                            {/*        <option value="">Sélectionner un module</option>*/}
                            {/*        {modules.map((module) => (*/}
                            {/*            <option key={module.moduleId} value={module.moduleId}>*/}
                            {/*                {module.moduleName}*/}
                            {/*            </option>*/}
                            {/*        ))}*/}
                            {/*    </select>*/}

                            {/*</div>*/}
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
