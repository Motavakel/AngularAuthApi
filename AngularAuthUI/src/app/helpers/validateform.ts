import { FormControl, FormGroup } from "@angular/forms";

export default class ValidateForm{
  
  static validateAllFormFileds(formGroup: FormGroup) {

    //برای هر ورودی که دارای نام است
    Object.keys(formGroup.controls).forEach(field => {

        //دریافت مقدار فیلد یا  کنترل با استفاده نام فیلد
        const control = formGroup.get(field);

        //در صورتی که از جنس کنترل باشد ،درتی میشه
        if (control instanceof FormControl) {
            control.markAsDirty({ onlySelf: true });
        }

        //در صورتی که فرم تو درتوی دیگری باشد دوباره متد براش اجرا میشه
        else if (control instanceof FormGroup) {
            this.validateAllFormFileds(control);
        }
    });
}
}
