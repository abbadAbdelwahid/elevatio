"use client"

import { useState } from "react"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"

export default function StudentListViewer() {
    const modules = ["Mathematics", "Web Development", "Java", "Data Structures"]
    const [selectedModule, setSelectedModule] = useState("Mathematics")

    const students = studentsData["GINF1"] || []

    const [grades, setGrades] = useState({})
    const [errors, setErrors] = useState({})

    const handleNoteChange = (apogee, value) => {
        let numericValue = parseFloat(value)

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

    return (
        <div className="p-6 space-y-6">
            <h1 className="text-2xl font-bold text-center">Enter Grades per Module</h1>

            <div className="flex justify-center gap-4">
                {modules.map((module) => (
                    <Button
                        key={module}
                        onClick={() => setSelectedModule(module)}
                        variant={selectedModule === module ? "default" : "outline"}
                    >
                        {module}
                    </Button>
                ))}
            </div>

            <Card>
                <CardContent className="overflow-x-auto">
                    <h2 className="text-xl font-semibold mb-4">Grades for {selectedModule}</h2>
                    <table className="min-w-full text-left border border-gray-300">
                        <thead className="bg-gray-100">
                        <tr>
                            <th className="p-2 border">Student ID</th>
                            <th className="p-2 border">Last Name</th>
                            <th className="p-2 border">First Name</th>
                            <th className="p-2 border">Birth Date</th>
                            <th className="p-2 border">Grade</th>
                        </tr>
                        </thead>
                        <tbody>
                        {students.map((student, index) => (
                            <tr key={index} className="border-t">
                                <td className="p-2 border">{student.apogee}</td>
                                <td className="p-2 border">{student.nom}</td>
                                <td className="p-2 border">{student.prenom}</td>
                                <td className="p-2 border">{student.dateNaissance}</td>
                                <td className="p-2 border">
                                    <div className="flex flex-col">
                                        {errors[student.apogee] && (
                                            <span className="text-sm text-red-500 mb-1">
                                                    {errors[student.apogee]}
                                                </span>
                                        )}
                                        <input
                                            type="number"
                                            min={0}
                                            max={20}
                                            step={0.5}
                                            className="note-input w-20 border px-2 py-1 rounded"
                                            value={grades[student.apogee] || ""}
                                            onChange={(e) =>
                                                handleNoteChange(student.apogee, e.target.value)
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
                        ))}
                        </tbody>
                    </table>
                </CardContent>
            </Card>
        </div>
    )
}
