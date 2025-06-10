
"use client"

import { useEffect, useState } from "react"
import { SearchHeader } from "@/components/layout/search-header"
import QuestionnairesManager from "@/components/admin/questionnaire-admin";

export default function CoursesPage() {
    const [questionnaires, setQuestionnaires] = useState([])
    const [loading, setLoading] = useState(true)

    const baseUrl = process.env.NEXT_PUBLIC_API_AUTH_URL
    const token = typeof window !== "undefined" ? localStorage.getItem("accessToken") : null

    // Fetch initiaux
    useEffect(() => {
        const fetchQuestionnaires = async () => {
            try {
                const res = await fetch(`${baseUrl}/questionnaires`, {
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: token ? `Bearer ${token}` : "",
                    },
                })
                if (res.ok) {
                    const data = await res.json()
                    setQuestionnaires(data)
                } else {
                    console.error("Erreur lors du fetch initial")
                }
            } catch (err) {
                console.error("Erreur serveur:", err)
            } finally {
                setLoading(false)
            }
        }

        fetchQuestionnaires()
    }, [])

    // Create ou Update
    const handleSave = async (questionnaire) => {
        const isNew = !questionnaire.id
        const url = isNew
            ? `${baseUrl}/questionnaires`
            : `${baseUrl}/questionnaires/${questionnaire.id}`

        const method = isNew ? "POST" : "PUT"

        const res = await fetch(url, {
            method,
            headers: {
                "Content-Type": "application/json",
                Authorization: token ? `Bearer ${token}` : "",
            },
            body: JSON.stringify(questionnaire),
        })

        if (res.ok) {
            const saved = await res.json()
            setQuestionnaires(prev =>
                isNew
                    ? [...prev, saved]
                    : prev.map(q => (q.id === saved.id ? saved : q))
            )
        }
    }

    const handleDelete = async (id) => {
        const res = await fetch(`${baseUrl}/questionnaires/${id}`, {
            method: "DELETE",
            headers: {
                Authorization: token ? `Bearer ${token}` : "",
            },
        })

        if (res.ok) {
            setQuestionnaires(prev => prev.filter(q => q.id !== id))
        }
    }

    const handleToggleStatus = async (id, newStatus) => {
        const res = await fetch(`${baseUrl}/questionnaires/${id}`, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                Authorization: token ? `Bearer ${token}` : "",
            },
            body: JSON.stringify({ status: newStatus }),
        })

        if (res.ok) {
            setQuestionnaires(prev =>
                prev.map(q => (q.id === id ? { ...q, status: newStatus } : q))
            )
        }
    }

    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                <QuestionnairesManager
                    questionnaires={questionnaires}
                    onSave={handleSave}
                    onDelete={handleDelete}
                    onToggleStatus={handleToggleStatus}
                    loading={loading}
                />
            </div>
        </div>
    )
}
