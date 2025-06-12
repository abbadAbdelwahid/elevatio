'use client'
import { useEffect, useState } from 'react'
import { Calendar } from "@/components/admin/dashboard/calendar"
import { RecentEvaluations } from "@/components/admin/dashboard/recent-evaluations"
import { StatsCards } from "@/components/admin/dashboard/stats-cards"
import { WelcomeBanner } from "@/components/admin/dashboard/welcome-banner"
import {SearchHeader} from "@/components/layout/search-header";
import {getRoleFromCookie, getUserIdFromCookie} from "@/lib/utils";


export default function DashboardPage() {
    const [userName, setUserName] = useState("")
    const [stats, setStats] = useState(null)
    const [evaluations, setEvaluations] = useState([])

    useEffect(() => {
        const token = localStorage.getItem("accessToken");

        const roleFromCookie = getRoleFromCookie();
        console.log(roleFromCookie);

        const fetchStatistics = async () => {
            const baseUrl = process.env.NEXT_PUBLIC_API_ANALYTICS_URL;
            const FiliereId=1
            const token = localStorage.getItem("accessToken"); // Assurez-vous que vous récupérez bien le token

            try {

                const res = await fetch(`${baseUrl}/api/StatFiliere/${FiliereId}`, {
                    method: "GET",
                    headers: {
                        "Authorization": `Bearer ${token}`,  // Ajoutez le token d'authentification si nécessaire
                        "Content-Type": "application/json"
                    }
                });

                if (!res.ok) {
                    const errorResponse = await res.json();  // Si l'API renvoie un message d'erreur
                    console.error('Backend Error:', errorResponse);
                    throw new Error(`Erreur lors de la récupération des statistiques: ${res.status} - ${errorResponse.message}`);
                }

                const data = await res.json();
                console.log('data stat: ', data);
                setStats(data); // Mettez à jour le state avec les données récupérées
            } catch (error) {
                console.error("Stats error:", error.message);
            }
        };

        const fetchUser = async () => {
            const baseUrl = process.env.NEXT_PUBLIC_API_AUTH_URL;
            try {
                const res = await fetch(`${baseUrl}/api/auth/me/info`, {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                if (!res.ok) throw new Error("Erreur lors de la récupération de l'utilisateur");
                const data = await res.json();
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

        fetchStatistics();
        fetchUser();
        // fetchRecentEvaluations();
    }, []);


    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-amber-50">
            {/* Main content */}
            <div className="container flex-1 p-8 ms-4">
                <SearchHeader />
                <WelcomeBanner fullName={userName} />
                <StatsCards stats={stats}  />
                <div className="mt-8 grid grid-cols-1 gap-8 lg:grid-cols-2">
                    <Calendar />
                    <RecentEvaluations  evaluations={evaluations}  />
                </div>
            </div>
        </div>

    )
}
