import {useState} from "react";

export default function FormComponen(){
    const [formValue, setFormValue] = useState([
        {pk: "url", value: ""},
        {pk: "shortUrl", value: ""},
        {pk: "linkPassword", value: ""},
        {pk: "userIdentifier", value: ""}
    ]);
    const [isUrlWrong, setIsUrlWrong] = useState(false);

    const handleChange = (pk, event) => {
        setFormValue(x => x.map( k => k.pk === pk ? {...k,value: event.target.value}:k));
    }

    const handleSubmit = event => {
        event.preventDefault()
        formValidator();
    }

    function formValidator(){
        const urlValidation = /^(https?:\/\/)?(www\.)?[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(:[0-9]{1,5})?(\/.*)?$/;
        if (urlValidation.test(formValue.find(x => x.pk === "url").value)){
            setIsUrlWrong(false);
        }else{
            setIsUrlWrong(true);
        }
    }

    return <div className="w-1/2 font-roboto-flex h-screen  bg-no-repeat bg-center">
            <form className="flex flex-col h-5/6 mx-34 justify-center pt-30">
                <p className="text-5xl font-medium text-center mb-12">LINK SHORTENER</p>
                <p className={`${!isUrlWrong && "hidden"} text-red-600`}>Url is invalid.</p>
                <input
                    type="text"
                    placeholder="URL"
                    name="url"
                    value={formValue.find(x => x.pk === "url").value}
                    onChange={(event) => handleChange("url",event)}
                    className={`${isUrlWrong && "border-red-600"} outline-none  border-b-1 text-black mb-8 text-xl`}
                />
                <input
                    type="text"
                    placeholder="SHORT URL"
                    name="shortUrl"
                    value={formValue.find(x => x.pk === "shortUrl").value}
                    onChange={(event) => handleChange("shortUrl",event)}
                    className={` border-b-1 text-black mb-8 text-xl outline-none`}
                />
                <input
                    type="password"
                    placeholder="LINK PASSWORD*"
                    name="linkPassword"
                    value={formValue.find(x => x.pk === "linkPassword").value}
                    onChange={(event) => handleChange("linkPassword",event)}
                    className={` border-b-1 text-black mb-8 text-xl outline-none `}
                />
                <input
                    type="password"
                    placeholder="USER IDENTIFIER"
                    name="userIdentifier"
                    value={formValue.find(x => x.pk === "userIdentifier").value}
                    onChange={(event) => handleChange("userIdentifier",event)}
                    className={` border-b-1 text-black mb-8 text-xl placeholder:pb-12 outline-none`}
                />
                <div className="flex justify-end">
                    <button
                        type="button"
                        onClick={handleSubmit}
                        className="bg-black w-1/3 text-white font-bold py-2 rounded-xl transition-colors duration-700 border-2 border-black hover:bg-white hover:text-black"
                    >
                        SUBMIT
                    </button>
                </div>
            </form>
    </div>
}
