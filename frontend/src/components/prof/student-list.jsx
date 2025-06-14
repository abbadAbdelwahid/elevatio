"use client"

import { useState, useEffect } from "react"
import { Card, CardContent } from "@/components/ui/card"

export default function StudentListViewer() {
    const [modules, setModules] = useState([])
    const [students, setStudents] = useState([])
    const [selectedModule, setSelectedModule] = useState("")
    const [grades, setGrades] = useState({})
    const [errors, setErrors] = useState({})

    // Charger les étudiants après sélection du module
    useEffect(() => {
        if (!selectedModule || modules.length === 0) return

        const moduleObj = modules.find(m => m.moduleName === selectedModule)
        if (!moduleObj) return

        const apiBase = process.env.NEXT_PUBLIC_API_AUTH_URL
        const filiereId = moduleObj.filiereId

        fetch(`${apiBase}/api/Etudiants/filiere/${filiereId}`)
            .then((res) => res.json())
            .then((data) => {
                console.log(`Étudiants pour filière ${filiereId} :`, data)
                if (Array.isArray(data)) {
                    setStudents(data)
                } else {
                    console.error("La réponse reçue n’est pas un tableau :", data)
                    setStudents([])
                }
            })
            .catch((err) => console.error("Erreur chargement étudiants :", err))
    }, [selectedModule, modules])

    // Charger les modules au démarrage
    useEffect(() => {
        const apiBase = process.env.NEXT_PUBLIC_API_COURSE_URL
        fetch(`${apiBase}/api/Module?filter=all`)
            .then((res) => res.json())
            .then((data) => {
                console.log("Modules chargés :", data)
                setModules(data)
                if (data.length > 0) setSelectedModule(data[0].moduleName)
            })
            .catch((err) => console.error("Erreur chargement modules :", err))
    }, [])

    const handleNoteChange = (apogee, value) => {
        const numericValue = parseFloat(value)

        if (isNaN(numericValue)) {
            setErrors((prev) => ({ ...prev, [apogee]: "Invalid value" }))
            setGrades((prev) => ({ ...prev, [apogee]: "" }))
        } else if (numericValue < 0 || numericValue > 20) {
            setErrors((prev) => ({ ...prev, [apogee]: "Value must be between 0 and 20" }))
            setGrades((prev) => ({ ...prev, [apogee]: numericValue }))
        } else {
            setErrors((prev) => ({ ...prev, [apogee]: "" }))
            setGrades((prev) => ({ ...prev, [apogee]: numericValue }))
        }
    }

    const handleSubmitGrades = async () => {
        const moduleObj = modules.find(m => m.moduleName === selectedModule)
        if (!moduleObj) return

        const apiBase = process.env.NEXT_PUBLIC_API_COURSE_URL
        const moduleId = moduleObj.moduleId

        const results = await Promise.allSettled(
            Object.entries(grades).map(async ([studentId, grade]) => {
                return fetch(`${apiBase}/api/Note`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        studentId: parseInt(studentId),
                        moduleId: moduleId,
                        grade: grade,
                        observation : "abbad"
                    })
                })
            })
        )

        const successCount = results.filter(r => r.status === "fulfilled").length
        alert(`${successCount} notes envoyées avec succès.`)
    }

    return (
        <div className="p-6 space-y-6">
            <h1 className="text-2xl font-bold text-center">Enter Grades per Module</h1>

            {/* Sélecteur de module */}
            <div className="flex justify-center">
                <select
                    value={selectedModule}
                    onChange={(e) => setSelectedModule(e.target.value)}
                    className="border border-gray-300 rounded px-3 py-2 text-sm"
                >
                    {modules.map((module) => (
                        <option key={module.moduleId} value={module.moduleName}>
                            {module.moduleName}
                        </option>
                    ))}
                </select>
            </div>

            <Card>
                <CardContent className="overflow-x-auto">
                    <h2 className="text-xl font-semibold mb-4">
                        Grades for <span className="text-blue-600">{selectedModule}</span>
                    </h2>
                    <table className="min-w-full text-left border border-gray-300">
                        <thead className="bg-gray-100">
                        <tr>
                            <th className="p-2 border">Student ID</th>
                            <th className="p-2 border">Last Name</th>
                            <th className="p-2 border">First Name</th>
                            <th className="p-2 border">Email</th>
                            <th className="p-2 border">Grade</th>
                        </tr>
                        </thead>
                        <tbody>
                        {Array.isArray(students) && students.length === 0 ? (
                            <tr>
                                <td colSpan={5} className="p-2 border text-center text-gray-500">
                                    Aucun étudiant pour ce module.
                                </td>
                            </tr>
                        ) : (
                            students.map((student, index) => (
                                <tr key={index} className="border-t">
                                    <td className="p-2 border">{student.id}</td>
                                    <td className="p-2 border">{student.lastName}</td>
                                    <td className="p-2 border">{student.firstName}</td>
                                    <td className="p-2 border">{student.email}</td>
                                    <td className="p-2 border">
                                        <div className="flex flex-col">
                                            {errors[student.id] && (
                                                <span className="text-sm text-red-500 mb-1">
                                                        {errors[student.id]}
                                                    </span>
                                            )}
                                            <input
                                                type="number"
                                                min={0}
                                                max={20}
                                                step={0.5}
                                                className="note-input w-20 border px-2 py-1 rounded"
                                                value={grades[student.id] || ""}
                                                onChange={(e) =>
                                                    handleNoteChange(student.id, e.target.value)
                                                }
                                                onKeyDown={(e) => {
                                                    if (e.key === "Enter") {
                                                        const inputs = document.querySelectorAll("input.note-input")
                                                        const currentIndex = Array.from(inputs).indexOf(e.target)
                                                        const nextInput = inputs[currentIndex + 1]
                                                        if (nextInput) nextInput.focus()
                                                    }
                                                }}
                                            />
                                        </div>
                                    </td>
                                </tr>
                            ))
                        )}
                        </tbody>
                    </table>

                    {/* Bouton d’envoi */}
                    <div className="mt-4 text-right">
                        <button
                            onClick={handleSubmitGrades}
                            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
                        >
                            Enregistrer les notes
                        </button>
                    </div>
                </CardContent>
            </Card>
        </div>
    )
}