import { Building, School, Star, Users, Smile } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";

export function StatsCards({ stats }) {
    return (
        <div className="grid grid-cols-1 gap-6 md:grid-cols-3 lg:grid-cols-4">
            {/* AVERAGE RATING */}
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
                            <span className="text-3xl font-bold text-gray-800">{stats?.averageRating ?? "--"}</span>
                            <span className="text-lg font-medium text-blue-600">/5</span>
                        </div>
                    </div>
                </CardContent>
            </Card>

            {/* TOTAL EVALUATIONS RECEIVED */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-purple-400 to-purple-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-purple-50/90 backdrop-blur-md">
                        <Users className="h-8 w-8 text-purple-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            TOTAL EVALUATIONS RECEIVED
                        </p>
                        <span className="text-3xl font-bold text-gray-800">{stats?.totalEvaluations ?? "--"}</span>
                    </div>
                </CardContent>
            </Card>

            {/* OVERALL SATISFACTION RATE */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-green-400 to-green-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-green-50/90 backdrop-blur-md">
                        <Smile className="h-8 w-8 text-green-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            OVERALL SATISFACTION RATE
                        </p>
                        <span className="text-3xl font-bold text-gray-800">{stats?.satisfactionRate ?? "--"}%</span>
                    </div>
                </CardContent>
            </Card>

            {/* EVALUATED COURSES */}
            <Card className="relative overflow-hidden border-0 bg-white shadow-[0_6px_30px_rgba(0,0,0,0.05)] transition-all hover:shadow-[0_10px_40px_rgba(0,0,0,0.1)]">
                <div className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-yellow-400 to-yellow-300" />
                <CardContent className="flex items-center gap-6 p-8">
                    <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-yellow-50/90 backdrop-blur-md">
                        <School className="h-8 w-8 text-yellow-600/90" />
                    </div>
                    <div className="space-y-2">
                        <p className="text-md font-bold uppercase tracking-widest text-[#0E2C75]">
                            EVALUATED COURSES
                        </p>
                        <div className="flex items-end gap-3">
                            <span className="text-3xl font-bold text-gray-800">{stats?.evaluatedCourses ?? "--"}</span>

                            <span className="text-lg font-medium text-yellow-600">/{stats?.totalCourses ?? "--"}</span>

                        </div>
                    </div>
                </CardContent>
            </Card>
        </div>
    );
}
