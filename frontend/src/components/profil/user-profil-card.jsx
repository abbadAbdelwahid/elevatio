import { useState } from "react";
import { Card, CardContent } from "@/components/ui/card";
import Image from "next/image";
import { FaCamera } from "react-icons/fa"; // Icône de caméra depuis react-icons

export function UserProfileCard({ user }) {
    const [profileImage, setProfileImage] = useState(user.image || "/placeholder.jpg");

    const handleImageChange = async (e) => {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onloadend = async () => {
                // Met à jour l'image de profil localement avec le fichier sélectionné
                setProfileImage(reader.result);

                // Créer un objet FormData pour envoyer le fichier
                const formData = new FormData();
                formData.append("image", file);  // Ajouter l'image à la FormData

                try {
                    const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
                    // Envoyer le fichier au serveur
                    const response = await fetch(`${baseUrl}/profile-image`, {
                        method: 'PATCH',  // ou 'POST' selon ton API
                        headers: {
                            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`, // Si tu utilises un token
                        },
                        body: formData,  // Envoyer la FormData avec l'image
                    });

                    if (!response.ok) {
                        throw new Error('Erreur lors de l\'envoi de l\'image');
                    }

                    const data = await response.json();
                    alert('Image de profil mise à jour avec succès');
                } catch (error) {
                    console.error('Erreur:', error);
                    alert('Échec de l\'envoi de l\'image');
                }
            };
            reader.readAsDataURL(file);  // Lire le fichier comme Data URL pour prévisualisation
        }
    };


    return (
        <Card className="overflow-hidden border-0 shadow-lg bg-white">
            <CardContent className="flex flex-col items-center p-8">
                {/* Photo de profil */}
                <div className="relative mb-6 h-40 w-40 overflow-hidden rounded-full border-4 border-purple-100">
                    <Image
                        src='/images/profil.svg'
                        alt="User profile"
                        width={160}
                        height={160}
                        className="object-cover"
                    />
                    {/* Icône de caméra */}
                    <label
                        htmlFor="profileImageInput"
                        className="absolute bottom-2 right-2 p-2 bg-white rounded-full border border-purple-200 shadow-lg cursor-pointer"
                    >
                        <FaCamera className="text-gray-600" size={24} />
                    </label>
                </div>

                {/* Formulaire de changement de photo */}
                <input
                    type="file"
                    accept="image/*"
                    onChange={handleImageChange}
                    className="hidden"
                    id="profileImageInput"
                />

                <h2 className="mb-2 text-xl font-semibold text-[#4a2a5a]">{user.firstName}</h2>
                <p className="text-md text-gray-500">{user.email}</p>
            </CardContent>
        </Card>
    );
}
