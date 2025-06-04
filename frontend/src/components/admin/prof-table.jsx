"use client"

import { useState } from "react"
import {BookOpen, ChevronDown, Code, File, FileText, Pencil, Trash} from "lucide-react"
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

const initialProfesseurs = [
    {
        id: 1,
        nom: "Ahmed Bouziane",
        email: "ahmed.bouziane@example.com",
        modules: ["Math", "Algorithmique"],
        rapports: ["rapport1.pdf", "rapport2.pdf"]
    },
    {
        id: 2,
        nom: "Nadia Lahlou",
        email: "nadia.lahlou@example.com",
        modules: ["Réseaux", "Sécurité"],
        rapports: ["rapportA.pdf"]
    }
]

export default function ProfsTable() {
    const [professeurs, setProfesseurs] = useState(initialProfesseurs)
    const [editProf, setEditProf] = useState(null)
    const [openDialog, setOpenDialog] = useState(false)

    const handleEdit = (prof) => {
        setEditProf(prof)
        setOpenDialog(true)
    }

    const handleDelete = (id) => {
        setProfesseurs(professeurs.filter(p => p.id !== id))
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        setProfesseurs(professeurs.map(p =>
            p.id === editProf.id ? editProf : p
        ))
        setOpenDialog(false)
    }

    return (
        <div className="rounded-lg border border-gray-200 bg-white shadow-sm overflow-x-auto">
            <Table>
                <TableHeader className="bg-[#f8f9fa]">
                    <TableRow className="border-b border-gray-200">
                        {["Professeur", "Email", "Modules", "Rapports", "Action"].map((header) => (
                            <TableHead key={header} className="px-4 py-3 text-sm font-medium text-gray-700 whitespace-nowrap">
                                {header}
                            </TableHead>
                        ))}
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {professeurs.map((prof) => (
                        <TableRow key={prof.id} className="border-b border-gray-100 hover:bg-gray-50">
                            <TableCell className="px-4 py-3 font-medium whitespace-nowrap">{prof.nom}</TableCell>
                            <TableCell className="px-4 py-3 whitespace-nowrap">{prof.email}</TableCell>
                            <TableCell className="px-4 py-3">
                                <details className="group cursor-pointer transition-all duration-300 ">
                                    <summary className="flex items-center gap-2 text-sm font-medium text-[#4a2a5a] hover:text-[#3a1a4a]">
      <span className="inline-flex h-6 w-6 items-center justify-center rounded-full bg-purple-100 transition-all group-open:bg-purple-200">
        <BookOpen className="h-4 w-4 text-purple-600" />
      </span>
                                        voir les modules <ChevronDown width={'14'} height={'14'} />
                                    </summary>
                                    <ul className="mt-3 space-y-2 border-t border-purple-50 pt-2  max-h-20 overflow-auto">
                                        {prof.modules.map((mod, i) => (
                                            <li
                                                key={i}
                                                className="flex items-center gap-2 rounded-lg bg-purple-50 px-3 py-1.5 text-sm text-purple-800 transition-all hover:bg-purple-100"
                                            >
                                                <Code className="h-4 w-4 text-purple-600" />
                                                {mod}
                                            </li>
                                        ))}
                                    </ul>
                                </details>
                            </TableCell>

                            <TableCell className="px-4 py-3">
                                <details className="group cursor-pointer transition-all duration-300">
                                    <summary className="flex items-center gap-2 text-sm font-medium text-[#4a2a5a] hover:text-[#3a1a4a]">
                                        <span className="inline-flex h-6 w-6 items-center justify-center rounded-full bg-blue-100 transition-all group-open:bg-blue-200">
                                              <FileText className="h-4 w-4 text-blue-600" />
                                        </span>
                                        voir les rapports <ChevronDown width={'14'} height={'14'} />
                                    </summary>
                                    <ul className="mt-3 space-y-2 border-t border-blue-50 pt-2 max-h-20 overflow-auto">
                                        {prof.rapports.map((r, i) => (
                                            <li
                                                key={i}
                                                className="flex items-center gap-2 rounded-lg bg-blue-50 px-3 py-1.5 text-sm text-blue-800 transition-all hover:bg-blue-100"
                                            >
                                                <File className="h-4 w-4 text-blue-600" />
                                                <a
                                                    href={`/${r}`}
                                                    className="hover:underline hover:decoration-blue-600"
                                                    download
                                                >
                                                    {r}
                                                </a>
                                            </li>
                                        ))}
                                    </ul>
                                </details>
                            </TableCell>
                            <TableCell className="px-4 py-3 whitespace-nowrap">
                                <div className="flex items-center gap-2">
                                    <Button variant="ghost" size="sm" onClick={() => handleEdit(prof)}>
                                        <Pencil className="h-4 w-4 text-blue-600" />
                                    </Button>
                                    <AlertDialog>
                                        <AlertDialogTrigger asChild>
                                            <Button variant="ghost" size="sm">
                                                <Trash className="h-4 w-4 text-red-600" />
                                            </Button>
                                        </AlertDialogTrigger>
                                        <AlertDialogContent className="sm:max-w-[600px] bg-white border-0">
                                            <AlertDialogHeader>
                                                <AlertDialogTitle>Confirmer la suppression</AlertDialogTitle>
                                                <AlertDialogDescription>
                                                    Supprimer le professeur "{prof.nom}" ?
                                                </AlertDialogDescription>
                                            </AlertDialogHeader>
                                            <AlertDialogFooter>
                                                <AlertDialogCancel>Annuler</AlertDialogCancel>
                                                <AlertDialogAction onClick={() => handleDelete(prof.id)} className="bg-red-600 hover:bg-red-700">
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
                            Modifier le professeur
                        </DialogTitle>
                    </DialogHeader>

                    <form onSubmit={handleSubmit} className="space-y-5">
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <Label>Nom complet</Label>
                                <Input value={editProf?.nom || ''} onChange={(e) => setEditProf({ ...editProf, nom: e.target.value })} />
                            </div>
                            <div className="space-y-2">
                                <Label>Email</Label>
                                <Input type="email" value={editProf?.email || ''} onChange={(e) => setEditProf({ ...editProf, email: e.target.value })} />
                            </div>
                            <div className="space-y-2 col-span-2">
                                <Label>Modules (séparés par virgule)</Label>
                                <Input value={editProf?.modules?.join(", ") || ''} onChange={(e) => setEditProf({ ...editProf, modules: e.target.value.split(',').map(m => m.trim()) })} />
                            </div>
                        </div>
                        <div className="flex justify-end gap-3">
                            <Button type="button" variant="outline" onClick={() => setOpenDialog(false)}>
                                Annuler
                            </Button>
                            <Button type="submit" className="bg-blue-600 hover:bg-blue-700">
                                Enregistrer
                            </Button>
                        </div>
                    </form>
                </DialogContent>
            </Dialog>
        </div>
    )
}