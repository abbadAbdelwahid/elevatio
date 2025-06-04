'use client'
import { useEffect, useState } from 'react'
import { Calendar } from "@/components/student/dashboard/calendar"
import { RecentEvaluations } from "@/components/student/dashboard/recent-evaluations"
import { StatsCards } from "@/components/student/dashboard/stats-cards"
import { WelcomeBanner } from "@/components/student/dashboard/welcome-banner"
import {SearchHeader} from "@/components/layout/search-header";

export default function DashboardPage() {
    const [stats, setStats] = useState({
        participationRate: 0,
        averageRating: 0,
        evaluatedCourses: 0,
        totalCourses: 0
    })
    const [evaluations, setEvaluations] = useState([])
  const  fullName="oussama"
    useEffect(() => {
        const fetchData = async () => {
            const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
            const resStats = await fetch(`${baseUrl}/statistics`)
            const statsData = await resStats.json()

            const resEval = await fetch(`${baseUrl}/recentEvaluations`)
            const evalData = await resEval.json()

            setStats(statsData)
            setEvaluations(evalData)
        }

        fetchData()
    }, [])


    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            {/* Main content */}
            <div className="container flex-1 p-8 ms-4">
                <SearchHeader />
                <WelcomeBanner fullName={fullName} />
                <StatsCards stats={stats} />
                <div className="mt-8 grid grid-cols-1 gap-8 lg:grid-cols-2">
                    <Calendar />
                    <RecentEvaluations  evaluations={evaluations} />
                </div>
            </div>
        </div>

    )
}
