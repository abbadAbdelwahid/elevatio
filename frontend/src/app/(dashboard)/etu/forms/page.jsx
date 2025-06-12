'use client';

import { useEffect, useState } from "react";
import { SearchHeader } from "@/components/layout/search-header";
import QuestionnairesManager from "@/components/student/forms/questionnaire-student";
import { getUserIdFromCookie } from "@/lib/utils";

export default function CoursesPage() {
    const [questionnaires, setQuestionnaires] = useState([]);
    const [loading, setLoading] = useState(true);
    const [refreshKey, setRefreshKey] = useState(0); // Clé de rafraîchissement

    const baseUrl = process.env.NEXT_PUBLIC_API_EVALUATION_URL;
    const token = typeof window !== "undefined" ? localStorage.getItem("accessToken") : null;

    const fetchQuestionnaires = async () => {
        try {
            const res = await fetch(`${baseUrl}/api/questionnaires/type/internal`, {
                headers: {
                    "Content-Type": "application/json",
                    Authorization: token ? `Bearer ${token}` : "",
                },
            });

            if (res.ok) {
                const data = await res.json();
                setQuestionnaires(data);
            } else {
                console.error("Erreur lors du fetch initial");
            }
        } catch (err) {
            console.error("Erreur serveur:", err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchQuestionnaires();
    }, [refreshKey]); // Déclenché quand refreshKey change

    const handleRefresh = () => {
        setRefreshKey(prev => prev + 1); // Force le rechargement
    };

    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                <QuestionnairesManager
                    questionnaires={questionnaires}
                    loading={loading}
                    onRefresh={handleRefresh} // Passe la fonction de rafraîchissement
                />
            </div>
        </div>
    );
}