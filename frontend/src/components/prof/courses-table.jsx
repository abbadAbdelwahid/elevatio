"use client"
import html2pdf from "html2pdf.js"
import { useEffect, useState } from "react"
import { Input } from "@/components/ui/input"
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

export function CoursesTable() {
    const [courses, setCourses] = useState([])
    const [selectedCourse, setSelectedCourse] = useState(null)
    const [isDialogOpen, setIsDialogOpen] = useState(false)

    // Récupérer les cours depuis l’API
    useEffect(() => {
        const apiBase = process.env.NEXT_PUBLIC_API_COURSE_URL
        fetch(`${apiBase}/api/Module?filter=all`)
            .then((res) => res.json())
            .then((data) => {
                if (Array.isArray(data)) {
                    console.log("✅ Modules reçus :", data)
                    setCourses(data)
                } else {
                    console.error("❌ Format inattendu :", data)
                    setCourses([])
                }
            })
            .catch((err) => {
                console.error("❌ Erreur de chargement :", err)
                setCourses([])
            })
    }, [])

    const handleDescriptionChange = (id, newDesc) => {
        setCourses((prev) =>
            prev.map((c) =>
                c.moduleId === id ? { ...c, moduleDescription: newDesc } : c
            )
        )
    }


    const generatePdfFromModule = async (moduleId, moduleName) => {
        try {
            const res = await fetch(`${process.env.NEXT_PUBLIC_API_ANALYTICS_URL}/api/ModuleReport/generateHtml/${moduleId}`)
            const html = await res.text()

            const container = document.createElement("div")
            container.innerHTML = html
            document.body.appendChild(container)

            await html2pdf()
                .set({
                    margin: 0.5,
                    filename: `Rapport_${moduleName}.pdf`,
                    image: { type: "jpeg", quality: 0.98 },
                    html2canvas: { scale: 2 },
                    jsPDF: { unit: "in", format: "a4", orientation: "portrait" },
                })
                .from(container)
                .save()

            document.body.removeChild(container)
        } catch (error) {
            console.error("Erreur PDF :", error)
            alert("❌ Impossible de générer le rapport PDF.")
        }
    }


    const openReportDialog = (course) => {
        setSelectedCourse(course)
        setIsDialogOpen(true)
    }

    return (
        <div className="rounded-lg border border-gray-200 bg-white shadow-sm">
            <Table>
                <TableHeader className="bg-[#f8f9fa]">
                    <TableRow className="border-b border-gray-200">
                        {[
                            "Course",
                            "Duration",
                            "Status",
                            "Description",
                            "Filiere",
                            "Teacher",
                            "Rapport",
                        ].map((header) => (
                            <TableHead
                                key={header}
                                className="px-4 py-3 text-sm font-medium text-gray-700"
                            >
                                {header}
                            </TableHead>
                        ))}
                    </TableRow>
                </TableHeader>

                <TableBody>
                    {Array.isArray(courses) && courses.length > 0 ? (
                        courses.map((course) => (
                            <TableRow
                                key={course.moduleId}
                                className="border-b border-gray-100 hover:bg-gray-50"
                            >
                                <TableCell className="px-4 py-3 text-sm font-medium text-gray-900">
                                    {course.moduleName}
                                </TableCell>
                                <TableCell className="px-4 py-3 text-sm text-gray-600">
                                    {course.moduleDuration} weeks
                                </TableCell>
                                <TableCell className="px-4 py-3">
                  <span
                      className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${
                          course.evaluated
                              ? "bg-green-100 text-green-800"
                              : "bg-yellow-100 text-yellow-800"
                      }`}
                  >
                    {course.evaluated ? "Terminé" : "En cours"}
                  </span>
                                </TableCell>
                                <TableCell className="px-4 py-3 text-sm text-gray-600">
                                    <Input
                                        type="text"
                                        value={course.moduleDescription}
                                        onChange={(e) =>
                                            handleDescriptionChange(course.moduleId, e.target.value)
                                        }
                                    />
                                </TableCell>
                                <TableCell className="px-4 py-3 text-sm text-gray-600">
                                    {course.filiereName}
                                </TableCell>
                                <TableCell className="px-4 py-3 text-sm text-gray-600">
                                    {course.teacherFullName}
                                </TableCell>
                                <TableCell className="px-4 py-3">
                                    <Button
                                        variant="outline"
                                        className="text-xs"
                                        onClick={() => generatePdfFromModule(course.moduleId, course.moduleName)}
                                    >
                                        Générer
                                    </Button>

                                </TableCell>
                            </TableRow>
                        ))
                    ) : (
                        <TableRow>
                            <TableCell colSpan={7} className="text-center text-sm py-4 text-gray-500">
                                Aucun cours disponible.
                            </TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>

            {/* Dialog de rapport */}
            <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
                <DialogContent>
                    <DialogHeader>
                        <DialogTitle>Rapport du cours</DialogTitle>
                    </DialogHeader>
                    {selectedCourse && (
                        <div className="space-y-2 text-sm text-gray-700">
                            <p><strong>Cours :</strong> {selectedCourse.moduleName}</p>
                            <p><strong>Description :</strong> {selectedCourse.moduleDescription}</p>
                            <p><strong>Durée :</strong> {selectedCourse.moduleDuration} semaines</p>
                            <p><strong>Statut :</strong> {selectedCourse.evaluated ? "Terminé" : "En cours"}</p>
                            <p><strong>Filière :</strong> {selectedCourse.filiereName}</p>
                            <p><strong>Enseignant :</strong> {selectedCourse.teacherFullName}</p>
                            <p className="mt-2 text-blue-500">📄 Rapport généré avec succès (simulation)</p>
                        </div>
                    )}
                </DialogContent>
            </Dialog>
        </div>
    )
}
