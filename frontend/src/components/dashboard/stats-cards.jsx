import { Building, School, Star } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"

export function StatsCards() {
    return (
        <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
            {/* Carte Participation */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0px_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0px_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-red-400 to-red-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-red-50/90 backdrop-blur-md">
                        <Building className="h-8 w-8 text-red-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            OVERALL PARTICIPATION RATE
                        </p>
                        <div className="flex items-end gap-3">
                            <span className="text-3xl font-bold text-gray-800">60%</span>

                        </div>
                    </div>
                </CardContent>
            </Card>

            {/* Carte Évaluation */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0px_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0px_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-blue-400 to-blue-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-blue-50/90 backdrop-blur-md">
                        <Star className="h-8 w-8 text-blue-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            AVERAGE RATING
                        </p>
                        <div className="flex items-end gap-3">
                            <span className="text-3xl font-bold text-gray-800">4.2</span>
                            <span className="text-lg font-medium text-blue-600">/5</span>
                        </div>
                    </div>
                </CardContent>
            </Card>

            {/* Carte Cours Évalués */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0px_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0px_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-green-400 to-green-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-green-50/90 backdrop-blur-md">
                        <School className="h-8 w-8 text-green-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            EVALUATED COURSES
                        </p>
                        <div className="flex items-end gap-3">
                            <span className="text-3xl font-bold text-gray-800">3</span>
                            <span className="text-lg font-medium text-green-600">/8</span>
                        </div>
                    </div>
                </CardContent>
            </Card>
        </div>
    )
}