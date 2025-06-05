'use client'
import { UserSettingsForm } from "@/components/profil/user-setting-form"
import { UserProfileCard } from "@/components/profil/user-profil-card"
import { UserInfoCard } from "@/components/profil/user-info-card"
import {SearchHeader} from "@/components/layout/search-header";
import { useEffect, useState } from "react";
export default function SettingsPage() {
    const [userDetails, setUserDetails] = useState({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        image:"",
        password:""
    });

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
                const res = await fetch(`${baseUrl}/profil`); // Remplacer par l'API backend
                const data = await res.json();
                console.log('profil: ',data[0])
                setUserDetails(data[0]);
            } catch (error) {
                console.error("Erreur de récupération des données", error);
            }
        };
        fetchUserData();
    }, []);
    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            <div className="container flex-1 p-8">

                <SearchHeader />

                <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
                    <div className="space-y-6 md:col-span-1">

                        <UserProfileCard user={userDetails} />
                        <UserInfoCard user={userDetails} />
                    </div>

                    <div className="md:col-span-2">
                        <UserSettingsForm  user={userDetails}/>
                    </div>
                </div>
            </div>
        </div>
    )
}
