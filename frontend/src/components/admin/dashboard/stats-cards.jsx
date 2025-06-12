
import { Building, School, Star } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"

export function StatsCards({ stats }) {
    console.log(stats);
    return (
        <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
            {/* Carte Satisfaction */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0px_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0px_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-red-400 to-red-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-red-50/90 backdrop-blur-md">
                        <Building className="h-8 w-8 text-red-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            SATISFACTION RATE
                        </p>
                        <div className="flex items-end gap-3">
                            <span className="text-3xl font-bold text-gray-800">
                                {stats?.satisfactionRate != null ? `${stats.satisfactionRate}%` : "72%"}
                            </span>
                        </div>
                    </div>
                </CardContent>
            </Card>

            {/* Carte Moyenne /20 */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0px_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0px_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-blue-400 to-blue-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-blue-50/90 backdrop-blur-md">
                        <Star className="h-8 w-8 text-blue-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            AVERAGE SCORE
                        </p>
                        <div className="flex items-end gap-3">
                            <span className="text-3xl font-bold text-gray-800">
                                {stats?.averageMoyenne != null ? stats.averageMoyenne : "14.6"}
                            </span>
                            <span className="text-lg font-medium text-blue-600">/20</span>
                        </div>
                    </div>
                </CardContent>
            </Card>

            {/* Carte Moyenne /5 */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0px_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0px_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-green-400 to-green-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-green-50/90 backdrop-blur-md">
                        <School className="h-8 w-8 text-green-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            AVERAGE RATING
                        </p>
                        <div className="flex items-end gap-3">
                            <span className="text-3xl font-bold text-gray-800">
                                {stats?.averageRating != null ? stats.averageRating : "4.2"}
                            </span>
                            <span className="text-lg font-medium text-blue-600">/5</span>
                        </div>
                    </div>
                </CardContent>
            </Card>


        </div>
    );
}
