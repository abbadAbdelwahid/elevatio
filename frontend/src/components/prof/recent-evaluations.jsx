"use client"

import { useEffect, useState } from "react"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from "recharts"

const fakeEvaluations = [
    { id: 1, course: "Java", professor: "Dr. Khadija El Mansouri", time: "Il y a 1h", teacherId: 101 },
    { id: 2, course: "Python", professor: "Dr. Khadija El Mansouri", time: "Il y a 3h", teacherId: 101 },
    { id: 3, course: "Python", professor: "Mme Sara El Amrani", time: "Il y a 4h", teacherId: 102 },
    { id: 4, course: "Java", professor: "Dr. Khadija El Mansouri", time: "Hier", teacherId: 101 },
]

const teacherId = 101 // Simulé comme étant le professeur connecté

export function RecentEvaluations () {
    const [evaluations, setEvaluations] = useState([])

    useEffect(() => {
        // Filtrer les évaluations selon le professeur connecté
        const filtered = fakeEvaluations.filter(e => e.teacherId === teacherId)
        setEvaluations(filtered)
    }, [])

    // Regrouper les stats
    const chartData = Object.values(
        evaluations.reduce((acc, evalItem) => {
            acc[evalItem.course] = acc[evalItem.course] || { module: evalItem.course, count: 0 }
            acc[evalItem.course].count++
            return acc
        }, {})
    )

    return (
        <Card className="border-0">
            <CardHeader>
                <CardTitle className="text-lg font-bold uppercase tracking-widest text-[#0E2C75]">
                    Évaluations récentes (Professeur)
                </CardTitle>
            </CardHeader>

            <CardContent className="space-y-6">
                <div className="space-y-4">
                    {evaluations.map((evaluation) => (
                        <div key={evaluation.id} className="flex items-center justify-between bg-blue-50 p-4 rounded-lg">
                            <div>
                                <h3 className="font-medium">{evaluation.course}</h3>
                                <p className="text-sm text-muted-foreground">{evaluation.professor}</p>
                            </div>
                            <p className="text-sm">{evaluation.time}</p>
                        </div>
                    ))}
                </div>

                <div className="mt-6">
                    <h3 className="text-md font-semibold mb-2 text-[#0E2C75]">Évaluations par module</h3>
                    <ResponsiveContainer width="100%" height={250}>
                        <BarChart data={chartData}>
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="module" />
                            <YAxis />
                            <Tooltip />
                            <Bar dataKey="count" fill="#0E2C75" radius={[4, 4, 0, 0]} />
                        </BarChart>
                    </ResponsiveContainer>
                </div>
            </CardContent>
        </Card>
    )
}
