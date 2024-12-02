<template>
  <el-dialog v-model="dialogVisible" :title="dialogTitle" width="35%" :before-close="handleClose">
    <el-form :model="ruleFroms" label-width="auto" style="max-width: 600px">
      <el-form-item label="名字" prop="name">
        <el-input v-model="ruleFroms.name" />
      </el-form-item>
      <el-form-item label="描述" prop="description">
        <el-input v-model="ruleFroms.description" />
      </el-form-item>
      <el-form-item label="权限ID" prop="permissionIds">
  <el-select v-model="state.ruleFroms.permissionIds" multiple placeholder="请选择权限">
    <el-option
      v-for="permission in state.permissions"
      :key="permission.id"
      :label="permission.name"
      :value="permission.id">
    </el-option>
  </el-select>
</el-form-item>
    </el-form>

    <template #footer>
      <div class="dialog-footer">
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="addCategory">提交</el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup>
import axios from '@/api/api_config';
import { reactive, toRefs, watch, inject, nextTick } from 'vue';
import { ElMessage } from 'element-plus';

const props = defineProps({
  dialogTitle: { type: String },
  tableRow: { type: Object }
});

const reload = inject("getList");

const state = reactive({
  dialogVisible: false,
  ruleFroms: {
    id: "",
    name: "",
    description: "",
    permissionIds: [],  // 保证这是一个数组
  },
  permissions: [], // 权限列表
});

nextTick(() => {
  console.log('Permissions Loaded:', state.permissions); // 确保 `el-select` 绑定的数据加载完成
});

const loadPermissions = async () => {
  try {
    const response = await axios.get('/Permissions');
    console.log('API Response:', response);

    if (response.status === 200 && Array.isArray(response.data)) {
      state.permissions = response.data.map(permission => ({
        id: permission.id, // 确保字段名与返回的数据匹配
        name: permission.name, // 确保字段名与返回的数据匹配
      }));
      console.log('Loaded Permissions:', state.permissions);
    } else {
      ElMessage.error("无法加载权限数据: 接口返回了意外的状态码 " + response.status);
    }
  } catch (error) {
    ElMessage.error("无法加载权限数据: " + error.message);
  }
};

loadPermissions(); // 加载权限列表

watch(
  () => props.tableRow,
  () => {
    state.ruleFroms = { ...props.tableRow };
  },
  { deep: true, immediate: true }
);

const dialogCategory = () => {
  state.dialogVisible = true;
};

const addCategory = () => {
  const param = {
    name: state.ruleFroms.name,
    description: state.ruleFroms.description,
    permissionIds: state.ruleFroms.permissionIds,
  };

  if (props.dialogTitle === "添加角色") {
    axios.post("/Roles", param)
      .then(() => {
        ElMessage.success("添加成功");
        state.dialogVisible = false;
        reload();
      })
      .catch((error) => {
        ElMessage.error("错误: " + error.response.data.message);
      });
  } else {
    axios.put(`/Roles/${props.tableRow.id}`, param).then(() => {
      ElMessage.success("修改成功");
      state.dialogVisible = false;
      reload();
    });
  }
};

const { dialogVisible, ruleFroms } = toRefs(state);

defineExpose({ dialogCategory });
</script>