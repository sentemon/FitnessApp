import {Component, HostListener} from '@angular/core';

@Component({
  selector: 'app-layout',
  template: `
    <div class="layout">
      <div class="sidebar-container flex" *ngIf="isSidebarOpen">
        <app-sidebar (linkClicked)="onSidebarLinkClick()"></app-sidebar>
        <button class="sidebar-toggle left" (click)="toggleSidebar()">←</button>
      </div>

      <div>
        <button class="sidebar-toggle right" *ngIf="!isSidebarOpen" (click)="toggleSidebar()">→</button>
        <app-main></app-main>
      </div>
    </div>
  `,
  styles: `
    .layout {
      display: flex;
      overflow: hidden;
    }

    .sidebar-container {
      position: relative;
    }

    .sidebar-toggle {
      position: absolute;
      top: 50%;
      background-color: #338874;
      border: none;
      color: white;
      padding: 0.5rem;
      font-size: 1.2rem;
      cursor: pointer;
      z-index: 10;
      border-radius: 0 5px 5px 0;
      transition: background-color 0.3s;

      &.left {
        right: 0;
        border-radius: 5px 0 0 5px;
      }

      &.right {
        left: 0;
        top: 50%;
      }
    }
  `
})
export class LayoutComponent {
  isSidebarOpen = true;

  constructor() {
    this.updateSidebarState();
  }

  @HostListener('window:resize')
  onResize() {
    this.updateSidebarState();
  }

  updateSidebarState() {
    this.isSidebarOpen = window.innerWidth >= 1024;
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  onSidebarLinkClick() {
    if (window.innerWidth < 1024) {
      this.isSidebarOpen = false;
    }
  }
}
