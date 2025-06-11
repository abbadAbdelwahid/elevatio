'use client'
import {Suspense, useEffect, useState} from "react";
import { CourseCard } from "./course-card";
import {getUserIdFromCookie} from "@/lib/utils";

export function CourseGrid({ courses }) {
    const [ratings, setRatings] = useState({}); // Objet pour stocker les ratings par module
    const fetchRatings = async () => {
        try {
            const respondentId = getUserIdFromCookie(); // Exemple de récupération de l'ID utilisateur
            const token = localStorage.getItem("accessToken");
            const baseUrl = process.env.NEXT_PUBLIC_API_EVALUATION_URL;

            const res = await fetch(`${baseUrl}/api/evaluations/getEvaluationsByRespondentId/${respondentId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });

            if (!res.ok) {
                console.error(`Erreur lors de la récupération des évaluations pour l'utilisateur ${respondentId}`);
                return;
            }

            const evaluations = await res.json();
            const ratingsData = {};

            // Filtrer les évaluations pour chaque module
            evaluations.forEach((evaluation) => {
                if (evaluation.type === "Module") {
                    ratingsData[evaluation.moduleId] = ratingsData[evaluation.moduleId] || [];
                    ratingsData[evaluation.moduleId].push(evaluation);
                }
            });
            setRatings(ratingsData);

        } catch (error) {
            console.error("Erreur lors de la récupération des évaluations", error);
        }
    };

    useEffect(() => {
        fetchRatings(); // Appel de la fonction pour récupérer les évaluations lorsque le composant se charge
    }, []); //

    console.log(ratings);
    return (
        <div className="p-14 max-h-[80vh] overflow-y-auto">

            {/* Grid Layout avec gestion de la taille dynamique */}
            <Suspense fallback={<p>Loading feed...</p>}>
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-4">
                {courses.map((course) => (
                           <CourseCard key={course.moduleId} course={course}  evaluation={ratings[course.moduleId] ||[]} />
                ))}
            </div>
            </Suspense >
        </div>
    );
}
