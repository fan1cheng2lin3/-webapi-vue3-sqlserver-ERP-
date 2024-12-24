<template>
 <el-card class="box-card">
  <template #header>
    <div class="card-header">
      <span>商品档案</span>
      <el-button type="primary" icon="CirclePlus" round @click="handleDialog(null)">添加</el-button>
    </div>
  </template>

  <!-- 设置固定高度并启用滚动条 -->
  <div style="max-height: 600px; overflow-y: auto;">
    <el-table :data="tableData.list" stripe style="width: 100%">
      <!-- 表格列内容 -->
      <el-table-column label="序号" width="80">
        <template #default="scope">
          {{ scope.$index + 1 }} <!-- 从1开始计数 -->
        </template>
      </el-table-column>

      <el-table-column prop="name" label="名称" width="180" />
      <el-table-column prop="product_code" label="商品编号" width="200" />
      <el-table-column prop="product_type" label="类别" width="200" />
      <el-table-column prop="supplier_name" label="供应商" width="200" />
      <el-table-column prop="unit_price" label="价格" width="200" />
      <el-table-column prop="count" label="数量" width="200" />

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
  </div>

  <!-- 添加商品对话框 -->
  <AddProuct ref="addCategory" :dialogTitle="dialogTitle" :tableRow="tableRow"></AddProuct>
</el-card>
</template>

<script setup>
import { reactive, onMounted, ref, provide } from 'vue';
import axios from "@/api/api_config";
import AddProuct from '@/components/AddProuct.vue';
import { isNull } from '@/utils/filter';
import { ElMessage, ElMessageBox } from 'element-plus';

// 响应式数据定义
const tableData = reactive({ list: [] });
const addCategory = ref(null); // 获取子属性组件实例
const dialogTitle = ref("");
const tableRow = ref({});

// 获取商品数据
const getList = () => {
  axios.get('/File_Management/product')
    .then((res) => {
      console.log(res.data); // 调试输出返回的数据
      tableData.list = res.data || [];  // 假设返回的数据是数组
    })
    .catch(err => {
      console.error("获取商品数据失败", err);
      ElMessage.error("获取商品数据失败");
    });
};

// 初始化加载数据
onMounted(() => {
  getList();  // 直接获取所有商品数据
});

// 打开添加或编辑的对话框
const handleDialog = (row) => {
  if (isNull(row)) {
    dialogTitle.value = "添加";
    tableRow.value = { id: "", name: "" }; // 初始化为空
  } else {
    dialogTitle.value = "修改";
    tableRow.value = JSON.parse(JSON.stringify(row)); // 深拷贝
  }

  addCategory.value.dialogCategory(); // 调用子组件的方法
};

// 删除商品
const open = (id) => {
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
      axios.delete(`/File_Management/deleteproduct/${id}`)
        .then(() => {
          ElMessage.success('删除成功');
          getList(); // 删除成功后重新获取数据
        })
      
    })
    .catch(() => {
      ElMessage.info('取消删除');
    });
};

provide("getList", getList);
</script>


<style scoped>
.el-header {
  position: relative;
  background-color: white;
  color: var(--el-text-color-primary);
  box-shadow: var(--el-box-shadow-dark);
}

.layout-container-demo .el-aside {
  color: var(--el-text-color-primary);
  background-color: #303133;
}

.layout-container-demo .el-menu {
  border-right: none;
}

.layout-container-demo .el-main {
  padding: 0;
  box-shadow: var(--el-box-shadow);
  margin: 6px 0px;
}

.layout-container-demo .toolbar {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  right: 20px;
}

.logo {
  height: 50px;
  color: white;
  text-align: center;
  line-height: 50px;
  font-weight: bold;
}

.layout-container-demo {
  height: 100vh;
}

.el-footer {
  box-shadow: var(--el-box-shadow);
}
</style>
