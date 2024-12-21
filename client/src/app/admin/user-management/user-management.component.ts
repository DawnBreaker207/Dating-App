import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { User } from '../../_models/user';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RoleModalComponent } from '../../modals/role-modal/role-modal.component';
import { tick } from '@angular/core/testing';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css',
})
export class UserManagementComponent implements OnInit {
  private adminService = inject(AdminService);
  users: User[] = [];
  private modalService = inject(BsModalService);
  bsModalRef: BsModalRef<RoleModalComponent> =
    new BsModalRef<RoleModalComponent>();
  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  openRolesModal(user: User) {
    const initialState: ModalOptions = {
      class: 'modal-lg',
      initialState: {
        title: 'User roles',
        username: user.username,
        selectedRole: [...user.roles],
        availableRole: ['Admin', 'Moderator', 'Member'],
        users: this.users,
        roleUpdated: false,
      },
    };
    this.bsModalRef = this.modalService.show(RoleModalComponent, initialState);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        if (this.bsModalRef.content && this.bsModalRef.content.rolesUpdated) {
          const selectedRoles = this.bsModalRef.content.selectedRoles;
          this.adminService
            .updateUserRoles(user.username, selectedRoles)
            .subscribe({
              next: (roles) => (user.roles = roles),
            });
        }
      },
    });
  }

  getUsersWithRoles() {
    this.adminService.getUserWithRole().subscribe({
      next: (users) => (this.users = users),
    });
  }
}
