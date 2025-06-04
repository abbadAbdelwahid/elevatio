"use client"

import { useState } from "react"
import { Pencil, Trash } from "lucide-react"
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

const initialCourses = [
    {
        id: 1,
        course: "C++",
        duration: 6,
        status: "in progress",
        professor: "Haddad Iwem",
        description: "Iwem Iwem",
        filiere: "GINF2",
        evaluation: 3.3
    },
    {
        id: 2,
        course: "Ethan Noah",
        duration: 4,
        status: "termine",
        professor: "Iwem Iwem",
        description: "Iwem",
        filiere: "GINF1",
        evaluation: 4.6
    }
]

export function CoursesTable() {
    const [courses, setCourses] = useState(initialCourses)
    const [editCourse, setEditCourse] = useState(null)
    const [openDialog, setOpenDialog] = useState(false)

    const handleEdit = (course) => {
        setEditCourse(course)
        setOpenDialog(true)
    }

    const handleDelete = (id) => {
        setCourses(courses.filter(course => course.id !== id))
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        setCourses(courses.map(course =>
            course.id === editCourse.id ? editCourse : course
        ))
        setOpenDialog(false)
    }

    return (
        <div className="rounded-lg border border-gray-200 bg-white shadow-sm">
            <Table>
                <TableHeader className="bg-[#f8f9fa]">
                    <TableRow className="border-b border-gray-200">
                        {["Course", "duration", "Status", "professor", "description", "Filiere", "evaluation", "Action"].map((header) => (
                            <TableHead key={header} className="px-4 py-3 text-sm font-medium text-gray-700">
                                {header}
                            </TableHead>
                        ))}
                    </TableRow>
                </TableHeader>

                <TableBody>
                    {courses.map((course) => (
                        <TableRow key={course.id} className="border-b border-gray-100 hover:bg-gray-50">
                            <TableCell className="px-4 py-3 text-sm font-medium text-gray-900">
                                {course.course}
                            </TableCell>

                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                {course.duration} weeks
                            </TableCell>

                            <TableCell className="px-4 py-3">
                                <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${
                                    course.status === 'in progress'
                                        ? 'bg-yellow-100 text-yellow-800'
                                        : 'bg-green-100 text-green-800'
                                }`}>
                                    {course.status}
                                </span>
                            </TableCell>

                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                {course.professor}
                            </TableCell>

                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                {course.description}
                            </TableCell>

                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                {course.filiere}
                            </TableCell>

                            <TableCell className="px-4 py-3">
                                <span className="text-sm font-medium text-blue-600">
                                    {course.evaluation}/5
                                </span>
                            </TableCell>

                            <TableCell className="px-4 py-3">
                                <div className="flex items-center gap-2">
                                    <Button
                                        variant="ghost"
                                        size="sm"
                                        className="text-gray-400 hover:text-blue-600"
                                        onClick={() => handleEdit(course)}
                                    >
                                        <Pencil className="h-4 w-4" />
                                    </Button>

                                    <AlertDialog >
                                        <AlertDialogTrigger asChild>
                                            <Button
                                                variant="ghost"
                                                size="sm"
                                                className="text-gray-400 hover:text-red-600"
                                            >
                                                <Trash className="h-4 w-4" />
                                            </Button>
                                        </AlertDialogTrigger>
                                        <AlertDialogContent className="sm:max-w-[600px] bg-white border-0">
                                            <AlertDialogHeader>
                                                <AlertDialogTitle>Confirmer la suppression</AlertDialogTitle>
                                                <AlertDialogDescription>
                                                    Êtes-vous sûr de vouloir supprimer définitivement le cours "{course.course}" ?
                                                </AlertDialogDescription>
                                            </AlertDialogHeader>
                                            <AlertDialogFooter>
                                                <AlertDialogCancel>Annuler</AlertDialogCancel>
                                                <AlertDialogAction
                                                    onClick={() => handleDelete(course.id)}
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

            <Dialog open={openDialog} onOpenChange={setOpenDialog}>
                <DialogContent className="sm:max-w-[600px] bg-white border-0">
                    <DialogHeader>
                        <DialogTitle className="text-lg font-semibold text-gray-900">
                            Modifier le cours
                        </DialogTitle>
                    </DialogHeader>

                    <form onSubmit={handleSubmit} className="space-y-5">
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <Label className="text-sm font-medium text-gray-700">Course</Label>
                                <Input
                                    className="focus:ring-2 focus:ring-blue-500"
                                    value={editCourse?.course || ''}
                                    onChange={(e) => setEditCourse({...editCourse, course: e.target.value})}
                                />
                            </div>

                            <div className="space-y-2">
                                <Label className="text-sm font-medium text-gray-700">Duration (weeks)</Label>
                                <Input
                                    type="number"
                                    className="focus:ring-2 focus:ring-blue-500"
                                    value={editCourse?.duration || ''}
                                    onChange={(e) => setEditCourse({...editCourse, duration: e.target.value})}
                                />
                            </div>

                            <div className="space-y-2">
                                <Label className="text-sm font-medium text-gray-700">Status</Label>
                                <select
                                    className="w-full rounded-md border-gray-300 focus:border-blue-500 focus:ring-2 focus:ring-blue-500"
                                    value={editCourse?.status || ''}
                                    onChange={(e) => setEditCourse({...editCourse, status: e.target.value})}
                                >
                                    <option value="in progress">In progress</option>
                                    <option value="termine">Terminé</option>
                                </select>
                            </div>

                            <div className="space-y-2">
                                <Label className="text-sm font-medium text-gray-700">Evaluation (/5)</Label>
                                <Input
                                    type="number"
                                    step="0.1"
                                    className="focus:ring-2 focus:ring-blue-500"
                                    value={editCourse?.evaluation || ''}
                                    onChange={(e) => setEditCourse({...editCourse, evaluation: e.target.value})}
                                />
                            </div>
                        </div>

                        <div className="flex justify-end gap-3">
                            <Button
                                type="button"
                                variant="outline"
                                className="border-gray-300 text-gray-700 hover:bg-gray-50"
                                onClick={() => setOpenDialog(false)}
                            >
                                Annuler
                            </Button>
                            <Button
                                type="submit"
                                className="bg-blue-600 hover:bg-blue-700 focus:ring-2 focus:ring-blue-500"
                            >
                                Enregistrer
                            </Button>
                        </div>
                    </form>
                </DialogContent>
            </Dialog>
        </div>
    )
}