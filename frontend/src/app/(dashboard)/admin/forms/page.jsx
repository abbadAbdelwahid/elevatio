'use client';

import { useEffect, useState } from "react";
import { SearchHeader } from "@/components/layout/search-header";
import QuestionnairesManager from "@/components/admin/questionnaire-admin";
import {getUserIdFromCookie} from "@/lib/utils";

export default function CoursesPage() {
    const [questionnaires, setQuestionnaires] = useState([]);
    const [loading, setLoading] = useState(true);

    const baseUrl = process.env.NEXT_PUBLIC_API_EVALUATION_URL;
    const token = typeof window !== "undefined" ? localStorage.getItem("accessToken") : null;

    // Récupérer les questionnaires initiaux
    useEffect(() => {
        const fetchQuestionnaires = async () => {
            try {
                const res = await fetch(`${baseUrl}/api/questionnaires/all/by-creator/${getUserIdFromCookie()}`, {
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: token ? `Bearer ${token}` : "",
                    },
                });

                if (res.ok) {
                    const data = await res.json();
                    console.log(data);
                    setQuestionnaires(data); // Sauvegarde les questionnaires récupérés
                } else {
                    console.error("Erreur lors du fetch initial");
                }
            } catch (err) {
                console.error("Erreur serveur:", err);
            } finally {
                setLoading(false);
            }
        };

        fetchQuestionnaires();
    }, []);

    const handleSave = async (questionnaire) => {
        const userId=getUserIdFromCookie().toLocaleString()
        const isNew = !questionnaire.questionnaireId; // Vérifier si c'est un nouveau questionnaire
        const url = isNew
            ? `${baseUrl}/api/questionnaires/add` // URL d'ajout d'un nouveau questionnaire
            : `${baseUrl}/api/questionnaires/update`; // URL de mise à jour d'un questionnaire existant

        const method = isNew ? "POST" : "PUT"; // Choisir la méthode appropriée (POST pour création, PUT pour mise à jour)

        // Ajouter un "standardQuestionId" égal à 1 à chaque question avant l'envoi
        const questionsWithStandardId = questionnaire.questions.map((question) => ({
            ...question, // Garder les propriétés existantes de la question
            standardQuestionId: 1, // Ajouter le champ "standardQuestionId" avec la valeur "1"
        }));

        // Structure du JSON à envoyer
        const dataToSend = {
            title: questionnaire.title, // Assurez-vous que le titre est défini
            typeInternalExternal: questionnaire.typeInternalExternal, // Type interne/externe
            typeModuleFiliere: questionnaire.typeModuleFiliere, // Type de module/filière
            filiereId: questionnaire.filiereId || 0, // Valeur par défaut pour `filiereId`
            moduleId: questionnaire.moduleId || 0, // Valeur par défaut pour `moduleId`
            creatorUserId:userId, // L'ID de l'utilisateur créateur
            questions: questionsWithStandardId, // Liste des questions avec `standardQuestionId` ajouté
        };

        // Envoi de la requête POST ou PUT
        const res = await fetch(url, {
            method,
            headers: {
                "Content-Type": "application/json",
                Authorization: token ? `Bearer ${token}` : "",
            },
            body: JSON.stringify(dataToSend),
        });

        // Vérification de la réponse
        if (res.ok) {
            const saved = await res.json();
            setQuestionnaires((prev) =>
                isNew
                    ? [...prev, saved] // Ajouter un nouveau questionnaire
                    : prev.map((q) => (q.questionnaireId === saved.questionnaireId ? saved : q)) // Mettre à jour un questionnaire existant
            );
        } else {
            // Affichage d'un message d'erreur si la requête échoue
            console.error("Erreur lors de l'enregistrement du questionnaire", await res.text());
        }
    };



    // Supprimer un questionnaire
    const handleDelete = async (id) => {
        const res = await fetch(`${baseUrl}/api/questionnaires/${id}`, {
            method: "DELETE",
            headers: {
                Authorization: token ? `Bearer ${token}` : "",
            },
        });

        if (res.ok) {
            setQuestionnaires((prev) => prev.filter((q) => q.questionnaireId !== id));
        }
    };

    // Activer ou désactiver un questionnaire
    const handleToggleStatus = async (id, newStatus) => {
        const res = await fetch(`${baseUrl}/api/questionnaires/${id}`, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                Authorization: token ? `Bearer ${token}` : "",
            },
            body: JSON.stringify({ status: newStatus }),
        });

        if (res.ok) {
            setQuestionnaires((prev) =>
                prev.map((q) => (q.questionnaireId === id ? { ...q, status: newStatus } : q))
            );
        }
    };

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
    );
}
