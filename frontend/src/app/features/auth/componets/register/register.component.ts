import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from "@angular/forms";
import {RouterLink} from "@angular/router";
import {AuthGuardService} from "../../services/auth-guard.service";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './../auth.scss'
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private authGuardService: AuthGuardService, private formBuilder: FormBuilder) {
    this.registerForm = this.formBuilder.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      username: ['', [Validators.required]], // ToDo: regex check
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
    }, { validators: this.passwordMatchValidator })
  }

  onRegister() {
    if (this.registerForm.valid) {
      // ToDo: save to cookie
      this.authGuardService.register(
        this.registerForm.value.firstName,
        this.registerForm.value.lastName,
        this.registerForm.value.username,
        this.registerForm.value.email,
        this.registerForm.value.password
      ).subscribe(result => {
        console.log(result);
      });
    } else {
      // ToDo
      console.log('Form is invalid', this.registerForm.errors);
    }
  }

  private passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }
}
