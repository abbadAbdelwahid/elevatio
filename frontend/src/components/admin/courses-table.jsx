"use client"

import { useState, useEffect } from "react"
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
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger
} from "@/components/ui/alert-dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"

export function CoursesTable() {
    const [courses, setCourses] = useState([])
    const [editCourse, setEditCourse] = useState(null)
    const [openDialog, setOpenDialog] = useState(false)
    const [selectedFiliere, setSelectedFiliere] = useState("all")
    const [filieres, setFilieres] = useState([])

    useEffect(() => {
        const fetchFilieres = async () => {
            try {
                const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;
                const token = localStorage.getItem("accessToken")

                const res = await fetch(`${baseUrl}/api/Filiere`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                })

                if (res.ok) {
                    const data = await res.json()
                    const nomsFilieres = data.map(item => item.filiereName)
                    setFilieres(nomsFilieres)
                }
            } catch (error) {
                console.error("Erreur lors de la récupération des filières:", error)
            }
        }

        fetchFilieres()
    }, [])

    useEffect(() => {
        const fetchCoursesAndEvaluations = async () => {
            try {
                const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL
                const token = localStorage.getItem("accessToken")

                let courseUrl = `${baseUrl}/api/Module?filter=All`
                if (selectedFiliere !== "all") {
                    courseUrl = `${baseUrl}/api/Module/filiere/${selectedFiliere}`
                }

                const courseRes = await fetch(courseUrl, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                })
                const coursesData = await courseRes.json()

                const evalRes = await fetch(`http://localhost:3001/evaluations`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                })
                const evalsData = await evalRes.json()

                const merged = coursesData.map(course => {
                    const evalItem = evalsData.find(e => e.courseId === course.id)
                    return {
                        ...course,
                        evaluation: evalItem ? evalItem.rating : 0
                    }
                })

                setCourses(merged)
            } catch (error) {
                console.error("Erreur lors du chargement des cours et évaluations:", error)
            }
        }

        fetchCoursesAndEvaluations()
    }, [selectedFiliere])

    const handleEdit = (course) => {
        setEditCourse(course)
        setOpenDialog(true)
    }

    const handleDelete = async (id) => {
        const token = localStorage.getItem("accessToken");
        try {
            const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;
            const res = await fetch(`${baseUrl}/api/Module/${id}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });

            if (!res.ok) {
                throw new Error("Erreur lors de la suppression");
            }

            // ✅ Rafraîchir les données après suppression
            const refreshRes = await fetch(`${baseUrl}/api/Module?filter=All`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });

            if (!refreshRes.ok) {
                throw new Error("Erreur lors du rafraîchissement");
            }

            const updatedCourses = await refreshRes.json();
            setCourses(updatedCourses);
        } catch (error) {
            console.error("Erreur delete :", error);
        }
    };


    const handleSubmit = async (e) => {
        e.preventDefault();
        const token = localStorage.getItem("accessToken");
        try {
            const baseUrl = process.env.NEXT_PUBLIC_API_AUTH_URL;
            const res = await fetch(`${baseUrl}/courses/${editCourse.id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify(editCourse),
            });

            if (!res.ok) {
                throw new Error("Erreur lors de la mise à jour");
            }

            const updatedCourse = await res.json();

            setCourses(courses.map(course =>
                course.id === updatedCourse.id ? updatedCourse : course
            ));
            setOpenDialog(false);
        } catch (error) {
            console.error("Erreur update :", error);
        }
    };

    return (
        <div className="rounded-lg border border-gray-200 bg-white shadow-sm">
            {/* Sélecteur de filière */}
            <div className="flex items-center gap-3 px-4 py-4">
                <Label htmlFor="filiere" className="text-sm font-medium text-gray-700">Filtrer par Filière :</Label>
                <select
                    id="filiere"
                    value={selectedFiliere}
                    onChange={(e) => setSelectedFiliere(e.target.value)}
                    className="rounded-md border border-gray-300 px-3 py-1 text-sm focus:ring-2 focus:ring-blue-500"
                >
                    <option value="all">All</option>
                    {filieres.map((filiere) => (
                        <option key={filiere} value={filiere}>{filiere}</option>
                    ))}
                </select>
            </div>

            {/* Tableau */}
            <Table>
                <TableHeader className="bg-[#f8f9fa]">
                    <TableRow className="border-b border-gray-200">
                        {["Course", "Duration", "Status", "Professor", "Filiere", "Evaluation", "Action"].map((header) => (
                            <TableHead key={header} className="px-4 py-3 text-sm font-medium text-gray-700">
                                {header}
                            </TableHead>
                        ))}
                    </TableRow>
                </TableHeader>

                <TableBody>
                    {courses.map((course) => (
                        <TableRow key={course.moduleId} className="border-b border-gray-100 hover:bg-gray-50">
                            <TableCell className="px-4 py-3 text-sm font-medium text-gray-900">
                                {course.moduleName}
                            </TableCell>
                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                {course.moduleDuration}
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
                                {course.teacherFullName}
                            </TableCell>
                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                {course.filiereName}
                            </TableCell>
                            <TableCell className="px-4 py-3">
                                <span className="text-sm font-medium text-blue-600">
                                    {course.evaluation}/5
                                </span>
                            </TableCell>
                            <TableCell className="px-4 py-3">
                                <div className="flex items-center gap-2">
                                    <Button variant="ghost" size="sm" onClick={() => handleEdit(course)}>
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
                                                    Supprimer le cours "{course.moduleName}" ?
                                                </AlertDialogDescription>
                                            </AlertDialogHeader>
                                            <AlertDialogFooter>
                                                <AlertDialogCancel>Annuler</AlertDialogCancel>
                                                <AlertDialogAction onClick={() => handleDelete(course.moduleId)} className="bg-red-600 hover:bg-red-700">
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

            {/* Dialog d'édition */}
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
                                <Label>Course</Label>
                                <Input value={editCourse?.moduleName || ''} onChange={(e) => setEditCourse({ ...editCourse, title: e.target.value })} />
                            </div>
                            <div className="space-y-2">
                                <Label>Duration</Label>
                                <Input value={editCourse?.moduleDuration || ''} onChange={(e) => setEditCourse({ ...editCourse, duration: e.target.value })} />
                            </div>
                            <div className="space-y-2">
                                <Label>Status</Label>
                                <select value={editCourse?.status || ''} onChange={(e) => setEditCourse({ ...editCourse, status: e.target.value })} className="w-full border rounded-md p-2">
                                    <option value="in progress">In progress</option>
                                    <option value="termine">Terminé</option>
                                </select>
                            </div>

                        </div>
                        <div className="flex justify-end gap-3">
                            <Button type="button" variant="outline" onClick={() => setOpenDialog(false)}>Annuler</Button>
                            <Button type="submit" className="bg-blue-600 hover:bg-blue-700">Enregistrer</Button>
                        </div>
                    </form>
                </DialogContent>
            </Dialog>
        </div>
    )
}
