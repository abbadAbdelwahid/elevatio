import { UserSettingsForm } from "@/components/profil/user-setting-form"
import { UserProfileCard } from "@/components/profil/user-profil-card"
import { UserInfoCard } from "@/components/profil/user-info-card"
import {SearchHeader} from "@/components/layout/search-header";

export default function SettingsPage() {
    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            <div className="container flex-1 p-8">

                <SearchHeader />

                <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
                    <div className="space-y-6 md:col-span-1">

                        <UserProfileCard />
                        <UserInfoCard />
                    </div>

                    <div className="md:col-span-2">
                        <UserSettingsForm />
                    </div>
                </div>
            </div>
        </div>
    )
}
