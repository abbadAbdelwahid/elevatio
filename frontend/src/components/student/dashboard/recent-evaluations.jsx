import { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

// Fonction pour rendre les étoiles

export function RecentEvaluations({ evaluations }) {
    const [modulesInfo, setModulesInfo] = useState([]);

    useEffect(() => {
        // Fonction pour récupérer les informations des modules et professeurs
        const fetchModulesInfo = async () => {
            try {
                // Récupérer les informations pour les 3 premiers modules
                const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL;
                const moduleIds = evaluations.slice(0, 3).map((evaluation) => evaluation.moduleId);
                const modulesData = await Promise.all(
                    moduleIds.map(async (moduleId) => {
                        const response = await fetch(`${baseUrl}/api/Module/${moduleId}`);
                        const module = await response.json();
                        return module;
                    })
                );
                setModulesInfo(modulesData);
            } catch (error) {
                console.error("Erreur lors de la récupération des informations du module", error);
            }
        };

        fetchModulesInfo();
    }, [evaluations]);
    // Tableau de couleurs fixes pour les évaluations
    const bgColors = ["bg-purple-100", "bg-green-100", "bg-blue-100"];

    return (
        <Card className={'border-0'}>
            <CardHeader>
                <CardTitle className="text-lg font-bold uppercase tracking-widest text-[#0E2C75]">Recent Evaluation</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
                {evaluations.slice(0, 3).map((evaluation, index) => {
                    const moduleInfo = modulesInfo[index];
                    return (
                        <div key={evaluation.evaluationId} className={`flex items-center justify-between rounded-lg ${bgColors[index]} p-4`}>
                            <div>
                                <h3 className="font-medium">{moduleInfo?.moduleName}</h3>
                                <p className="text-sm text-muted-foreground">{moduleInfo?.teacherFullName}</p>
                            </div>
                            <div className="text-right">
                                <p className="font-medium">{new Date(evaluation.evaluatedAt).toLocaleDateString()}</p>
                                {/* Affichage du score sous forme d'étoiles */}
                                <div className="flex gap-1">
                                    {[1, 2, 3, 4, 5].map((star) => (
                                        <button
                                            key={star}
                                            type="button"
                                            onClick={() => setRating(star)}
                                            className={`text-2xl ${star <= evaluation.score ? 'text-yellow-400' : 'text-gray-300'} hover:text-yellow-500`}
                                        >
                                            ★
                                        </button>
                                    ))}
                                </div>
                            </div>
                        </div>
                    );
                })}
            </CardContent>
        </Card>
    );
}
