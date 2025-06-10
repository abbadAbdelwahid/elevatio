import Image from "next/image";

// Fonction pour formater la date actuelle
function getFormattedDate() {
    const today = new Date();
    const options = { year: "numeric", month: "long", day: "numeric" };
    return today.toLocaleDateString(undefined, options); // utilise la locale du navigateur
}

export function WelcomeBanner({ fullName }) {
    return (
        <div className="mb-8">
            <div className="mt-6 overflow-hidden rounded-xl bg-gradient-to-r from-[#866BA2] to-[#4D2C5E] text-white">
                <div className="flex items-center justify-between">
                    <div className="space-y-2 p-10 ms-3">
                        <p className="text-sm text-white/80">{getFormattedDate()}</p>
                        <h2 className="text-2xl font-bold">Welcome back, {fullName}!</h2>
                        <p className="text-sm">Always stay updated in your student portal</p>
                    </div>
                    <div className="flex pt-0  me-20">

                        <Image
                            src="/images/dashboard/admin.svg"
                            alt="Student illustration"
                            width={270}
                            height={230}
                            className="object-contain"
                        />
                    </div>
                </div>
            </div>
        </div>
    );
}
