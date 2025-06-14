"use client"

import { useEffect, useState } from "react"
import { Building, School, Star, Users, Smile, AlertTriangle } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"
import {getUserIdFromCookie} from "@/lib/utils";

export function StatsCards({ ensId }) {
    const [stats, setStats] = useState(null)
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState(false)

    const API_URL = process.env.NEXT_PUBLIC_API_ANALYTICS_URL

    useEffect(() => {
        const ensId = getUserIdFromCookie()
        const fetchStats = async () => {
            try {
                const res = await fetch(`${API_URL}/api/StatEns/${ensId}`)
                if (!res.ok) throw new Error("Erreur de récupération")
                const data = await res.json()
                setStats(data)
            } catch (err) {
                console.error(err)
                setError(true)
            } finally {
                setLoading(false)
            }
        }

        if (ensId) fetchStats()
    }, [ensId, API_URL])

    if (loading) return <p className="text-gray-500">Chargement des statistiques...</p>
    if (error || !stats) return <p className="text-red-600">Erreur lors du chargement des statistiques.</p>

    return (
        <div className="grid grid-cols-1 gap-6 md:grid-cols-3 lg:grid-cols-4">
            {/* Average */}
            <StatCard
                icon={Star}
                title="COURSES AVERAGE RATING"
                value={stats.averageM}
                suffix="/5"
                gradient="from-blue-400 to-blue-300"
            />

            {/* Pass rate */}
            <StatCard
                icon={Users}
                title="PASS RATE"
                value={stats.passRate}
                suffix="%"
                gradient="from-purple-400 to-purple-300"
            />

            {/* Note max */}
            <StatCard
                icon={Smile}
                title="NOTE MAX"
                value={stats.noteMax}
                suffix="/20"
                gradient="from-green-400 to-green-300"
            />

            {/* Note min */}
            <StatCard
                icon={School}
                title="NOTE MIN"
                value={stats.noteMin}
                suffix="/20"
                gradient="from-yellow-400 to-yellow-300"
            />

            {/* Feedback positif */}
            <StatCard
                icon={Smile}
                title="POSITIVE FEEDBACK"
                value={stats.positiveFeedBackPct}
                suffix="%"
                gradient="from-green-400 to-green-300"
            />

            {/* Feedback négatif */}
            <StatCard
                icon={AlertTriangle}
                title="NEGATIVE FEEDBACK"
                value={stats.negativeFeedBackPct}
                suffix="%"
                gradient="from-red-400 to-red-300"
            />
        </div>
    )
}

function StatCard({ icon: Icon, title, value, suffix, gradient }) {
    return (
        <Card className="relative overflow-hidden border-0 bg-white shadow-[0_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0_10px_40px_rgba(0,0,0,0.1)]">
            <div className={`absolute inset-x-0 top-0 h-1 bg-gradient-to-r ${gradient}`} />
            <CardContent className="flex items-center gap-6 p-8">
                <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-gray-50/90 backdrop-blur-md">
                    <Icon className="h-8 w-8 text-gray-700" />
                </div>
                <div className="space-y-2">
                    <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                        {title}
                    </p>
                    <div className="flex items-end gap-2">
                        <span className="text-3xl font-bold text-gray-800">{value ?? "--"}</span>
                        {suffix && <span className="text-lg font-medium text-gray-600">{suffix}</span>}
                    </div>
                </div>
            </CardContent>
        </Card>
    )
}
