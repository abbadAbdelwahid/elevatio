'use client'
import { useEffect, useState } from 'react'
import { Calendar } from "@/components/student/dashboard/calendar"
import { RecentEvaluations } from "@/components/student/dashboard/recent-evaluations"
import { StatsCards } from "@/components/student/dashboard/stats-cards"
import { WelcomeBanner } from "@/components/student/dashboard/welcome-banner"
import {SearchHeader} from "@/components/layout/search-header";
import {getRoleFromCookie} from "@/lib/utils";

export default function DashboardPage() {
    const [userName, setUserName] = useState("")
    const [stats, setStats] = useState({
        participationRate: 0,
        averageRating: 0,
        evaluatedCourses: 0,
        totalCourses: 0
    })
    const [evaluations, setEvaluations] = useState([])

    useEffect(() => {
        const token = localStorage.getItem("accessToken");
        const baseUrl = process.env.NEXT_PUBLIC_API_AUTH_URL;
        const roleFromCookie = getRoleFromCookie();
        console.log(roleFromCookie);

        // const fetchStatistics = async () => {
        //     try {
        //         const res = await fetch(`${baseUrl}/statistics`, {
        //             method: "GET",
        //             headers: {
        //                 "Content-Type": "application/json",
        //                 Authorization: token ? `Bearer ${token}` : ""
        //             }
        //         });
        //         if (!res.ok) throw new Error("Erreur lors de la récupération des statistiques");
        //         const data = await res.json();
        //         setStats(data);
        //     } catch (error) {
        //         console.error("Stats error:", error.message);
        //     }
        // };

        const fetchUser = async () => {
            try {
                const res = await fetch(`${baseUrl}/api/${roleFromCookie}s/me`, {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                if (!res.ok) throw new Error("Erreur lors de la récupération de l'utilisateur");
                const data = await res.json();
                console.log("User data:", data);
                setUserName(`${data.firstName || ""} ${data.lastName || ""}`.trim());
            } catch (error) {
                console.error("User error:", error.message);
            }
        };

        // const fetchRecentEvaluations = async () => {
        //     try {
        //         const res = await fetch(`${baseUrl}/recentEvaluations`, {
        //             method: "GET",
        //             headers: {
        //                 "Content-Type": "application/json",
        //                 Authorization: token ? `Bearer ${token}` : ""
        //             }
        //         });
        //         if (!res.ok) throw new Error("Erreur lors de la récupération des évaluations");
        //         const data = await res.json();
        //         setEvaluations(data);
        //     } catch (error) {
        //         console.error("Evaluations error:", error.message);
        //     }
        // };

        // fetchStatistics();
        fetchUser();
        // fetchRecentEvaluations();
    }, []);


    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            {/* Main content */}
            <div className="container flex-1 p-8 ms-4">
                <SearchHeader />
                <WelcomeBanner fullName={userName} />
                <StatsCards stats={stats} />
                <div className="mt-8 grid grid-cols-1 gap-8 lg:grid-cols-2">
                    <Calendar />
                    <RecentEvaluations  evaluations={evaluations} />
                </div>
            </div>
        </div>

    )
}
