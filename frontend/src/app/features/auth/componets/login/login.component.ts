import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgIf} from "@angular/common";
import {RouterLink} from "@angular/router";
import {AuthGuardService} from "../../services/auth-guard.service";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIf,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './../auth.scss'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(private authGuardService: AuthGuardService, private formBuilder: FormBuilder) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    })
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      this.authGuardService.login(this.loginForm.value.email, this.loginForm.value.password);
    } else {
      console.error('Form is invalid', this.loginForm.errors);
    }
  }
}
