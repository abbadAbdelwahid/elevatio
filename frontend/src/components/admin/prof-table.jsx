'use client'
import { useState, useEffect } from "react";
import { BookOpen, ChevronDown, Code, File, FileText, Pencil, Trash } from "lucide-react";
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
import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "@/components/ui/alert-dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";


    export default function ProfsTable() {
        const [professeurs, setProfesseurs] = useState([]);
        const [modules, setModules] = useState([]);
        const [modulesUns, setModulesUns] = useState([]);
        const [rapports, setRapports] = useState([]);
        const [editProf, setEditProf] = useState({
            teacherId: "",
            moduleIds: []
        });
        const [openDialog, setOpenDialog] = useState(false);
        const [isLoading, setIsLoading] = useState(false);
        const token = localStorage.getItem("accessToken");

        const fetchData = async () => {
            setIsLoading(true);
            try {
                const baseUrl = process.env.NEXT_PUBLIC_API_AUTH_URL;
                const courseBaseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;

                const [profsRes, modulesRes, moduleUnssigned] = await Promise.all([
                    fetch(`${baseUrl}/api/Enseignants`, {
                        headers: {Authorization: token ? `Bearer ${token}` : ""}
                    }),
                    fetch(`${courseBaseUrl}/api/Module?filter=All`, {
                        headers: {Authorization: token ? `Bearer ${token}` : ""}
                    }),
                    fetch(`${courseBaseUrl}/api/Module/unassigned`, {
                        headers: {Authorization: token ? `Bearer ${token}` : ""}
                    })
                ]);

                if (profsRes.ok) setProfesseurs(await profsRes.json());
                if (modulesRes.ok) setModules(await modulesRes.json());
                if (moduleUnssigned.ok) setModulesUns(await moduleUnssigned.json());
            } catch (error) {
                console.error("Erreur:", error);
            } finally {
                setIsLoading(false);
            }
        };

        useEffect(() => {
            fetchData();
        }, [token]);

        const handleEdit = (prof) => {
            const assignedModuleIds = modules
                .filter(mod => mod.teacherId === prof.id)
                .map(mod => mod.id);

            setEditProf({
                id: prof.id,
                modules: assignedModuleIds
            });

            setOpenDialog(true);
        };

        const handleDelete = async (id) => {
            const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;
            try {
                const res = await fetch(`${baseUrl}/api/Enseignants/${id}`, {
                    method: 'DELETE',
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: token ? `Bearer ${token}` : "",
                    }
                });
                if (res.ok) {
                    setProfesseurs(professeurs.filter(p => p.id !== id));
                }
            } catch (error) {
                console.error("Erreur:", error);
            }
        };

        const handleSubmit = async (e) => {
            e.preventDefault();
            if (!editProf) return;

            setIsLoading(true);
            const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;

            try {
                const payload = {
                    teacherId: editProf.id,
                    moduleIds: editProf.modules
                };

                const res = await fetch(`${baseUrl}/api/Module/assign`, {
                    method: 'POST',
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: token ? `Bearer ${token}` : "",
                    },
                    body: JSON.stringify(payload)
                });

                if (!res.ok) throw new Error("Échec de l'affectation des modules");

                await fetchData();
                setOpenDialog(false);
            } catch (error) {
                console.error("Erreur lors de l'affectation des modules", error);
            } finally {
                setIsLoading(false);
            }
        };

        return (
            <div className="rounded-lg border border-gray-200 bg-white shadow-sm overflow-x-auto">
                <Table>
                    <TableHeader className="bg-[#f8f9fa]">
                        <TableRow className="border-b border-gray-200">
                            {["Professeur", "Email", "Modules", "Rapports", "Action"].map((header) => (
                                <TableHead key={header}
                                           className="px-4 py-3 text-sm font-medium text-gray-700 whitespace-nowrap">
                                    {header}
                                </TableHead>
                            ))}
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {isLoading && !professeurs.length ? (
                            <TableRow>
                                <TableCell colSpan={5} className="text-center py-4">
                                    Chargement...
                                </TableCell>
                            </TableRow>
                        ) : professeurs.map((prof) => (
                            <TableRow key={prof.id} className="border-b border-gray-100 hover:bg-gray-50">
                                <TableCell className="px-4 py-3 font-medium whitespace-nowrap">
                                    {prof.firstName} {prof.lastName}
                                </TableCell>
                                <TableCell className="px-4 py-3 whitespace-nowrap">{prof.email}</TableCell>
                                <TableCell className="px-4 py-3">
                                    <details className="group cursor-pointer transition-all duration-300">
                                        <summary
                                            className="flex items-center gap-2 text-sm font-medium text-[#4a2a5a] hover:text-[#3a1a4a]">
                                        <span
                                            className="inline-flex h-6 w-6 items-center justify-center rounded-full bg-purple-100 transition-all group-open:bg-purple-200">
                                            <BookOpen className="h-4 w-4 text-purple-600"/>
                                        </span>
                                            voir les modules <ChevronDown width={'14'} height={'14'}/>
                                        </summary>
                                        <ul className="mt-3 space-y-2 border-t border-purple-50 pt-2 max-h-20 overflow-auto">
                                            {modules
                                                .filter(mod => mod.teacherId?.toString() === prof.id.toString())
                                                .map((mod, i) => (
                                                    <li
                                                        key={i}
                                                        className="flex items-center gap-2 rounded-lg bg-purple-50 px-3 py-1.5 text-sm text-purple-800 transition-all hover:bg-purple-100"
                                                    >
                                                        <Code className="h-4 w-4 text-purple-600"/>
                                                        {mod.moduleName}
                                                    </li>
                                                ))}
                                            {modules.filter(mod => mod.teacherId?.toString() === prof.id.toString()).length === 0 && (
                                                <li className="text-sm text-gray-500 px-3 py-1.5">
                                                    Aucun module affecté
                                                </li>
                                            )}
                                        </ul>
                                    </details>
                                </TableCell>
                                <TableCell className="px-4 py-3">
                                    <details className="group cursor-pointer transition-all duration-300">
                                        <summary
                                            className="flex items-center gap-2 text-sm font-medium text-[#4a2a5a] hover:text-[#3a1a4a]">
                                        <span
                                            className="inline-flex h-6 w-6 items-center justify-center rounded-full bg-blue-100 transition-all group-open:bg-blue-200">
                                            <FileText className="h-4 w-4 text-blue-600"/>
                                        </span>
                                            voir les rapports <ChevronDown width={'14'} height={'14'}/>
                                        </summary>
                                        <ul className="mt-3 space-y-2 border-t border-blue-50 pt-2 max-h-20 overflow-auto">
                                            {rapports.filter(r => r.profId === prof.id).map((r, i) => (
                                                <li
                                                    key={i}
                                                    className="flex items-center gap-2 rounded-lg bg-blue-50 px-3 py-1.5 text-sm text-blue-800 transition-all hover:bg-blue-100"
                                                >
                                                    <File className="h-4 w-4 text-blue-600"/>
                                                    <a
                                                        href={r.url}
                                                        className="hover:underline hover:decoration-blue-600"
                                                        download
                                                    >
                                                        {r.name}
                                                    </a>
                                                </li>
                                            ))}
                                            {rapports.filter(r => r.profId === prof.id).length === 0 && (
                                                <li className="text-sm text-gray-500 px-3 py-1.5">
                                                    Aucun rapport disponible
                                                </li>
                                            )}
                                        </ul>
                                    </details>
                                </TableCell>
                                <TableCell className="px-4 py-3 whitespace-nowrap">
                                    <div className="flex items-center gap-2">
                                        <Button
                                            variant="ghost"
                                            size="sm"
                                            onClick={() => handleEdit(prof)}
                                            disabled={isLoading}
                                        >
                                            <Pencil className="h-4 w-4 text-blue-600"/>
                                        </Button>
                                        <AlertDialog>
                                            <AlertDialogTrigger asChild>
                                                <Button variant="ghost" size="sm" disabled={isLoading}>
                                                    <Trash className="h-4 w-4 text-red-600"/>
                                                </Button>
                                            </AlertDialogTrigger>
                                            <AlertDialogContent className="sm:max-w-[600px] bg-white border-0">
                                                <AlertDialogHeader>
                                                    <AlertDialogTitle>Confirmer la suppression</AlertDialogTitle>
                                                    <AlertDialogDescription>
                                                        Supprimer le professeur "{prof.firstName} {prof.lastName}" ?
                                                    </AlertDialogDescription>
                                                </AlertDialogHeader>
                                                <AlertDialogFooter>
                                                    <AlertDialogCancel>Annuler</AlertDialogCancel>
                                                    <AlertDialogAction
                                                        onClick={() => handleDelete(prof.id)}
                                                        className="bg-red-600 hover:bg-red-700"
                                                    >
                                                        Supprimer
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

                <Dialog open={openDialog} onOpenChange={setOpenDialog}>
                    <DialogContent className="sm:max-w-[600px] bg-white border-0">
                        <DialogHeader>
                            <DialogTitle className="text-lg font-semibold text-gray-900">
                                Affecter les modules à {editProf?.firstName} {editProf?.lastName}
                            </DialogTitle>
                        </DialogHeader>

                        <form onSubmit={handleSubmit} className="space-y-5">
                            <div className="space-y-2">
                                <Label>Modules</Label>
                                <div className="grid grid-cols-2 gap-2 max-h-48 overflow-y-auto p-2 border rounded-md">
                                    {modulesUns.length === 0 ? (
                                        <p className="col-span-2 text-sm text-gray-500">Aucun module à affecter</p>
                                    ) : (
                                        modulesUns.map((mod) => {
                                            const selectedModules = Array.isArray(editProf?.modules) ? editProf.modules : [];
                                            const isChecked = selectedModules.includes(mod.moduleId);

                                            return (
                                                <label key={mod.moduleId} className="flex items-center space-x-2">
                                                    <input
                                                        type="checkbox"
                                                        checked={isChecked}
                                                        onChange={(e) => {
                                                            let updatedModules;
                                                            if (e.target.checked) {
                                                                updatedModules = [...selectedModules, mod.moduleId];
                                                            } else {
                                                                updatedModules = selectedModules.filter(id => id !== mod.moduleId);
                                                            }

                                                            setEditProf((prev) => ({
                                                                ...prev,
                                                                modules: updatedModules
                                                            }));
                                                        }}
                                                    />
                                                    <span>{mod.moduleName}</span>
                                                </label>
                                            );
                                        })
                                    )}
                                </div>
                            </div>


                            <div className="flex justify-end gap-3">
                                <Button type="button" variant="outline" onClick={() => setOpenDialog(false)}>
                                    Annuler
                                </Button>
                                <Button type="submit" className="bg-blue-600 hover:bg-blue-700 text-white">
                                    Enregistrer
                                </Button>
                            </div>
                        </form>
                    </DialogContent>
                </Dialog>
            </div>
        );

}












