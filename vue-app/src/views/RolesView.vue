<template>
    <el-card class="box-card">
      <template #header>
        <div class="card-header">
          <span>角色管理</span>
  
          <el-button type="primary" icon="CirclePlus" round @click="handleDialog(null)">添加角色</el-button>
  
        </div>
      </template>
  
  
      <el-table :data="tableData.list" stripe style="width: 100%">
  <!-- 行号列 -->
  <el-table-column label="序号" width="80">
    <template #default="scope">
      {{ scope.$index + 1 }} <!-- 从1开始计数 -->
    </template>
  </el-table-column>
  
  <el-table-column prop="name" label="名字" width="80" />
  <el-table-column prop="description" label="描述" width="120" />
  <el-table-column label="权限" width="200">
  <template #default="scope">
    <div>
      <!-- 遍历每个角色的权限 -->
      <div v-for="(permissionObj, index) in scope.row.rolePermissions" :key="index">
        <!-- 显示权限的名称 -->
        {{ permissionObj.permission.name }}
      </div>
    </div>
  </template>
</el-table-column>
  <el-table-column fixed="right" label="操作" width="180">
    <template #default="scope">
      <el-button type="success" size="small" @click="handleDialog(scope.row)">
        修改
      </el-button>
      <el-button type="danger" size="small" @click="open(scope.row.id)">
        删除
      </el-button>
    </template>
  </el-table-column>
  
  
  </el-table>
  
  <AddUsers ref="addCategory"
  :dialogTitle="dialogTitle"
  :tableRow="tableRow"></AddUsers>
    </el-card>
  
  </template>
  
  
  <script setup>
  
  
  import { reactive,onMounted,ref,provide } from 'vue';
  import axios from "@/api/api_config";
  import AddUsers from '@/components/AddRoles.vue';
  import { isNull } from '@/utils/filter';
  import { ElMessage, ElMessageBox } from 'element-plus'
  
  ///----------------------------------
  const tableData = reactive({list:[]})
  const addCategory = ref(null) //获取子属性组件实例
  const dialogTitle = ref("")
  const tableRow = ref({})
  
  
  //-----------------------------------
  
  
  onMounted(()=>{
  getList()
  })
  
  //获取分类的信息
  const getList=()=>{
  return axios.get('/Roles').then((res)=>{
    tableData.list = res.data
    console.log(res.data);
  });
  };
  
  provide("getList",getList);
  
  //打开分类页
  const handleDialog = (row) => {
  if (isNull(row)) {
    dialogTitle.value = "添加角色";
    tableRow.value = { id: "", name: "" }; // 初始化为空
  } else {
    dialogTitle.value = "修改角色信息";
    tableRow.value = JSON.parse(JSON.stringify(row)); // 深拷贝
  }
  
  addCategory.value.dialogCategory(); // 调用子组件的方法
  };
  
  
  
  const open=(id)=>{
  ElMessageBox.confirm(
    '确定要删除吗?',
    '你确定要删除吗',
    {
      confirmButtonText: '确认',
      cancelButtonText: '取消',
      type: 'warning',
    }
  )
    .then(() => {
      axios.delete(`/Roles/${id}`).then(()=>{
        ElMessage({
        type: 'success',
        message: '删除成功',
      });
      getList();
      });
    })
    .catch(() => {
      ElMessage({
        type: 'info',
        message: '取消删除',
      })
    })
  }

  
  </script>