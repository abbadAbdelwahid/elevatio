// loading.js
export default function Loading() {
    return (
        <div className="h-full w-full flex items-center justify-center  bg-opacity-90 backdrop-blur-sm ">
            <div className="text-center space-y-4">
                {/* Spinner moderne */}
                <div className="relative w-20 h-20 mx-auto">
                    <div className="absolute inset-0 border-4 border-blue-200 rounded-full"></div>
                    <div className="absolute inset-0 border-4 border-transparent border-t-blue-600 rounded-full animate-spin"></div>
                </div>

                {/* Texte animé */}
                <div className="space-y-2">
                    <h2 className="text-lg font-semibold text-gray-700">
                        Chargement des cours
                    </h2>
                    <p className="text-sm text-gray-500">
                        Préparation du contenu...
                    </p>
                </div>

                {/* Barre de progression */}
                <div className="w-48 h-1.5 bg-gray-100 rounded-full mx-auto overflow-hidden">
                    <div className="h-full bg-blue-600 animate-progress origin-left" />
                </div>
            </div>
        </div>
    );
}