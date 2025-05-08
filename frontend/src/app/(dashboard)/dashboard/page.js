import { Calendar } from "@/components/dashboard/calendar"
import { RecentEvaluations } from "@/components/dashboard/recent-evaluations"
import { StatsCards } from "@/components/dashboard/stats-cards"
import { WelcomeBanner } from "@/components/dashboard/welcome-banner"
import {SearchHeader} from "@/components/search-header";

export default function DashboardPage() {

  const  fullName="oussama"


    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-amber-50">
            {/* Main content */}
            <div className="container flex-1 p-8 ms-4">
                <SearchHeader />
                <WelcomeBanner fullName={fullName} />
                <StatsCards />
                <div className="mt-8 grid grid-cols-1 gap-8 lg:grid-cols-2">
                    <Calendar />
                    <RecentEvaluations />
                </div>
            </div>
        </div>

    )
}
