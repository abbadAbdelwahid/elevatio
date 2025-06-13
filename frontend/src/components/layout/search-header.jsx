'use client'
import {useEffect, useState} from "react";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarImage, AvatarFallback } from "@/components/ui/avatar";
import { Input } from "@/components/ui/input";
import { Notifications } from "@/components/notification/notifications";
import { Bell, Mail, Search } from "lucide-react";
import {getRoleFromCookie, getUserIdFromCookie} from "@/lib/utils";  // Icône de recherche

export function SearchHeader() {
    const [isNotificationsOpen, setIsNotificationsOpen] = useState(false); // État pour gérer l'ouverture du panel
    const [notifications, setNotifications] = useState([
        {
            id: 1,
            message: "Un nouveau questionnaire interne est disponible à remplir.",
            date: "2025-06-12 10:15",
            read: false,
        },
        {
            id: 2,
            message: "Votre réponse au questionnaire ‘Satisfaction Module Java’ a été enregistrée avec succès.",
            date: "2025-06-11 14:30",
            read: false,
        },
        {
            id: 3,
            message: "Un rappel : vous avez un questionnaire en attente dans la filière Génie Informatique.",
            date: "2025-06-10 08:45",
            read: false,
        },
    ]);
    const [userDetails, setUserDetails] = useState('');
    const [role, setRole] = useState({});
    const baseUrl = process.env.NEXT_PUBLIC_API_AUTH_URL;
    useEffect(() => {
        const fetchUserData = async () => {
            try {

                // Si l'API nécessite un token d'authentification (par exemple dans un en-tête Authorization)
                const token = localStorage.getItem("accessToken"); // ou une autre méthode pour récupérer le token
                const roleFromCookie = getRoleFromCookie()
                const userIdFromCookie=getUserIdFromCookie()
                setRole(roleFromCookie)
                const test=`${baseUrl}/${roleFromCookie}s/me`
                let res=undefined;
                if(roleFromCookie==='Admin'){
                    res = await fetch(`${baseUrl}/api/auth/me/info`, {
                        method: "GET",
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    });
                }else{
                    res = await fetch(`${baseUrl}/api/${roleFromCookie}s/me`, {
                        method: "GET",
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    });
                }


                // Vérifier si la réponse est correcte avant de la traiter
                if (!res.ok) {
                    throw new Error("Erreur de récupération des données");
                }

                const data = await res.json();
                // Si la réponse est un tableau, on prend le premier élément, sinon on l'utilise directement
                setUserDetails(data[0] || data); // Si c'est un tableau, on prend le premier élément

            } catch (error) {
                console.error("Erreur de récupération des données", error);
            }
        };

        fetchUserData();
    }, []);
    // Ouvrir ou fermer le panel des notifications
    const toggleNotifications = () => {
        setIsNotificationsOpen(!isNotificationsOpen);
    };

    // Marquer une notification comme lue
    const markAsRead = (id) => {
        const updatedNotifications = notifications.map((notification) =>
            notification.id === id ? { ...notification, read: true } : notification
        );
        setNotifications(updatedNotifications);
    };

    // Marquer toutes les notifications comme lues
    const markAllAsRead = () => {
        const updatedNotifications = notifications.map((notification) => ({
            ...notification,
            read: true,
        }));
        setNotifications(updatedNotifications);
    };

    // Fermer le panel des notifications
    const closeNotificationsPanel = () => {
        setIsNotificationsOpen(false);
    };

    // Calculer le nombre de notifications non lues
    const unreadCount = notifications.filter((notification) => !notification.read).length;

    return (
        <div className="mb-8 bg-white rounded-3xl">
            <div className="flex items-center justify-between px-10 py-4">
                <h3 className="text-md font-semibold">Welcome to Elevatio </h3>
                <div className="flex items-center gap-4">
                    <div className="relative">
                        <Input placeholder="Search" className="w-74 rounded-full bg-gray-100 pl-4 pr-10 border-0" />
                        <Button size="icon" variant="ghost" className="absolute right-0 top-0 h-full rounded-full">
                            <Search className="h-4 w-4" />
                        </Button>
                    </div>
                    <Button size="icon" variant="outline" className="rounded-full">
                        <Mail className="h-4 w-4" />
                    </Button>

                    {/* Icône de notification avec badge */}
                    <Button size="icon" variant="outline" className="relative rounded-full" onClick={toggleNotifications}>
                        <Bell className="h-2 w-2" />
                        {unreadCount > 0 && (
                            <div className="absolute top-0 right-0 bg-red-500 text-white text-xs rounded-full w-4 h-4 flex items-center justify-center">
                                {unreadCount}
                            </div>
                        )}
                    </Button>

                    <Avatar>
                        <AvatarImage  src={`${baseUrl}${userDetails.profileImageUrl}`} />
                        <AvatarFallback>JD</AvatarFallback>
                    </Avatar>
                </div>
            </div>

            {/* Panel des notifications (si ouvert) */}
            {isNotificationsOpen && (
                <div className="fixed top-0 right-0 w-96 h-full bg-blue-100 shadow-lg z-50 p-6">
                    <Notifications
                        notifications={notifications}
                        markAsRead={markAsRead}
                        markAllAsRead={markAllAsRead}
                        closePanel={closeNotificationsPanel}
                    />
                </div>
            )}
        </div>
    );
}
